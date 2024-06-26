// Services/CarService.cs
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPlatform.Services
{
  public class CarService : ICarService
  {
    private readonly CarRentalContext _context;

    public CarService(CarRentalContext context)
    {
      _context = context;
    }

    public async Task<List<Car>> GetAllCarsAsync()
    {
      return await _context.Cars.ToListAsync();
    }

    public async Task<PagedResult<Car>> GetPagedCarsAsync(int pageNumber, int pageSize)
    {
      var query = _context.Cars.AsQueryable();

      var totalCount = await query.CountAsync();
      var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

      return new PagedResult<Car>
      {
        Items = items,
        TotalCount = totalCount,
        PageNumber = pageNumber,
        PageSize = pageSize
      };
    }

    public async Task<Car?> GetCarByIdAsync(int id)
    {
      var car = await _context.Cars
                            .Include(c => c.Rentals) // 加载相关的 Rentals
                            .FirstOrDefaultAsync(c => c.Id == id);
      return car;
    }

    public async Task<Car> CreateCarAsync(Car car)
    {

      _context.Cars.Add(car);
      await _context.SaveChangesAsync();
      return car;
    }

    public async Task UpdateCarAsync(Car updatedCar, Car existingCar)
    {
      // _context.Entry(car).State = EntityState.Modified;
      // await _context.SaveChangesAsync();

      _context.Entry(existingCar).CurrentValues.SetValues(updatedCar);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteCarAsync(int id)
    {
      var car = await _context.Cars.FindAsync(id);
      if (car != null)
      {
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
      }
    }
  }
}
