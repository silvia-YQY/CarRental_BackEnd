// Services/ICarService.cs
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;

namespace CarRentalPlatform.Services
{
  public interface ICarService
  {
    Task<List<Car>> GetAllCarsAsync();
    Task<PagedResult<Car>> GetPagedCarsAsync(int pageNumber, int pageSize);
    Task<Car?> GetCarByIdAsync(int id);
    Task<Car> CreateCarAsync(Car car);
    Task UpdateCarAsync(Car car, Car existingCar);
    Task DeleteCarAsync(int id);
  }
}
