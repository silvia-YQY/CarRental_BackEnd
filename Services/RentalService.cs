// Services/RentalService.cs
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalPlatform.Services
{
  public class RentalService : IRentalService
  {
    private readonly CarRentalContext _context;
    private readonly ILogger<RentalService> _logger; // 添加 ILogger 字段

    public RentalService(CarRentalContext context, ILogger<RentalService> logger)
    {
      _context = context;
      _logger = logger;

    }

    public async Task<IEnumerable<Rental>> GetAllRentalsAsync()
    {
      return await _context.Rentals
                          .Include(r => r.Car)   // 包含关联的车辆信息
                          .Include(r => r.User)  // 包含关联的用户信息
                          .ToListAsync();
    }

    public async Task<PagedResult<Rental>> GetPagedRentalAsync(int pageNumber, int pageSize)
    {
      var query = _context.Rentals.AsQueryable();

      var totalCount = await query.CountAsync();
      var items = await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .Include(r => r.Car)
                              .Include(r => r.User)
                              .ToListAsync();

      return new PagedResult<Rental>
      {
        Items = items,
        TotalCount = totalCount,
        PageNumber = pageNumber,
        PageSize = pageSize
      };
    }

    public async Task<Rental> GetRentalByIdAsync(int id)
    {
      var rental = await _context.Rentals.Include(r => r.Car)
                                          .Include(r => r.User)
                                          .FirstOrDefaultAsync(r => r.Id == id);
      if (rental == null)
      {
        throw new KeyNotFoundException($"Rental with ID {id} not found.");
      }
      return rental;
    }

    public async Task<Rental?> CreateRentalAsync(Rental rental)
    {
      using (var transaction = _context.Database.BeginTransaction())
      {

        // 查找现有的Car和User
        var car = await _context.Cars.FindAsync(rental.CarId);
        var user = await _context.Users.FindAsync(rental.UserId);

        if (car == null || user == null)
        {
          throw new KeyNotFoundException($"Car or User not found");
        }

        if (!car.Available_Now)
        {
          transaction.Rollback(); // 回滚事务
          throw new Exception("Car not available now");
        }

        // 检查当前车辆在给定时间段内是否有重叠的租赁记录
        var overlappingRental = await _context.Rentals
            .Where(r => r.CarId == rental.CarId &&
                        ((rental.StartDate >= r.StartDate && rental.StartDate <= r.EndDate) ||
                         (rental.EndDate >= r.StartDate && rental.EndDate <= r.EndDate) ||
                         (rental.StartDate <= r.StartDate && rental.EndDate >= r.EndDate)))
            .FirstOrDefaultAsync();


        if (overlappingRental != null)
        {
          throw new Exception("The current vehicle is occupied at the current time");
        }

        // 费用计算
        var fee = CalculateRentalFee(rental, car);

        if (fee != rental.Fee)
        {
          throw new Exception($"The current amount should be {fee}");

        }


        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        transaction.Commit(); // 提交事务
        return rental;

      }
    }

    public async Task<Rental?> UpdateRentalAsync(Rental rental)
    {

      // _context.Entry(rental).State = EntityState.Modified;
      // Update existingRental using DbContext Update method
      // _context.Update(rental);
      // 检查当前车辆在给定时间段内是否有重叠的租赁记录
      var overlappingRental = await _context.Rentals
          .Where(r => r.CarId == rental.CarId &&
                      ((rental.StartDate >= r.StartDate && rental.StartDate <= r.EndDate) ||
                       (rental.EndDate >= r.StartDate && rental.EndDate <= r.EndDate) ||
                       (rental.StartDate <= r.StartDate && rental.EndDate >= r.EndDate)))
          .FirstOrDefaultAsync();


      if (overlappingRental != null)
      {
        throw new Exception("The current vehicle is occupied at the current time");
      }
      // 查找现有的Car和User
      var car = await _context.Cars.FindAsync(rental.CarId);
      var user = await _context.Users.FindAsync(rental.UserId);

      if (car == null || user == null)
      {
        throw new KeyNotFoundException($"Car or User not found");
      }

      // 费用计算
      var fee = CalculateRentalFee(rental, car);

      if (fee != rental.Fee)
      {
        throw new Exception($"The current amount should be {fee}");

      }


      await _context.SaveChangesAsync();
      return rental;

    }

    public async Task<Rental?> UpdateRentalStatusAsync(Rental rental)
    {


      await _context.SaveChangesAsync();
      return rental;

    }

    private decimal CalculateRentalFee(Rental rental, Car car)
    {
      var dailyRate = car.Price_Per_Day;
      var totalDays = (rental.EndDate - rental.StartDate).Days;
      return dailyRate * totalDays;
    }

    public async Task<bool> DeleteRentalAsync(int id)
    {
      var rental = await _context.Rentals.FindAsync(id);

      if (rental != null)
      {
        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();
        return true;
      }
      else
      {
        return false;
      }

    }
  }

}

