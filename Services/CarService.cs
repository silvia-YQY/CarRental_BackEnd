// Services/CarService.cs
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

    public async Task<Car> GetCarByIdAsync(int id)
    {
      var car = await _context.Cars.FindAsync(id);
      if (car == null)
      {
        throw new KeyNotFoundException($"Car with ID {id} not found.");
      }
      return car;
    }

    public async Task<Car> CreateCarAsync(Car car)
    {

      _context.Cars.Add(car);
      await _context.SaveChangesAsync();
      return car;
    }

    public async Task UpdateCarAsync(Car car)
    {
      _context.Entry(car).State = EntityState.Modified;
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
