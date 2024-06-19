// Services/RentalService.cs
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPlatform.Services
{
  public class RentalService : IRentalService
  {
    private readonly CarRentalContext _context;

    public RentalService(CarRentalContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Rental>> GetAllRentalsAsync()
    {
      return await _context.Rentals
                          .Include(r => r.Car)   // 包含关联的车辆信息
                          .Include(r => r.User)  // 包含关联的用户信息
                          .ToListAsync();
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

    public async Task<Rental?> CreateRentalAsync(RentalCreateDto rentalDto)
    {
      using (var transaction = _context.Database.BeginTransaction())
      {
        try
        {
          // 查找现有的Car和User
          var car = await _context.Cars.FindAsync(rentalDto.CarId);
          var user = await _context.Users.FindAsync(rentalDto.UserId);

          if (car == null || user == null)
          {
            throw new KeyNotFoundException($"Car or User not found");
          }


          var rental = new Rental
          {
            CarId = rentalDto.CarId,
            UserId = rentalDto.UserId,
            StartDate = rentalDto.StartDate,
            EndDate = rentalDto.EndDate,
            Fee = rentalDto.Fee,
            Status = rentalDto.Status,
            Car = car,  // 设置关联的Car实体
            User = user // 设置关联的User实体

          };
          _context.Rentals.Add(rental);
          await _context.SaveChangesAsync();
          Console.WriteLine($"CreateRentalAsync==={rental.Id}");
          // 2. 其他相关操作

          transaction.Commit(); // 提交事务
          return rental;

        }

        catch (Exception ex)
        {
          transaction.Rollback(); // 回滚事务
          throw new Exception("Failed to create rental", ex);
        }

      }
    }

    public async Task UpdateRentalAsync(RentalCreateDto rentalDto)
    {
      var rental = await _context.Rentals.FindAsync(rentalDto.Id);
      if (rental == null)
      {
        throw new KeyNotFoundException($"Rental with ID {rentalDto.Id} not found.");
      }

      rental.CarId = rentalDto.CarId;
      rental.UserId = rentalDto.UserId;
      rental.StartDate = rentalDto.StartDate;
      rental.EndDate = rentalDto.EndDate;
      rental.Fee = rentalDto.Fee;
      rental.Status = rentalDto.Status;

      _context.Entry(rental).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public async Task DeleteRentalAsync(int id)
    {
      var rental = await _context.Rentals.FindAsync(id);
      if (rental != null)
      {
        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();
      }
    }
  }
}
