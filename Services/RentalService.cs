// Services/RentalService.cs
using CarRentalPlatform.Models;
using Microsoft.EntityFrameworkCore;

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
        try
        {
          // 查找现有的Car和User
          var car = await _context.Cars.FindAsync(rental.CarId);
          var user = await _context.Users.FindAsync(rental.UserId);

          if (car == null || user == null)
          {
            throw new KeyNotFoundException($"Car or User not found");
          }

          _context.Rentals.Add(rental);
          await _context.SaveChangesAsync();
          Console.WriteLine($"CreateRentalAsync==={rental.Id}");

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

    public async Task UpdateRentalAsync(Rental rental)
    {

      // _context.Entry(rental).State = EntityState.Modified;
      // Update existingRental using DbContext Update method
      // _context.Update(rental);
      await _context.SaveChangesAsync();
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
