// Services/IRentalService.cs
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;

namespace CarRentalPlatform.Services
{
  public interface IRentalService
  {
    Task<IEnumerable<Rental>> GetAllRentalsAsync();
    Task<Rental> GetRentalByIdAsync(int id);
    Task<Rental?> CreateRentalAsync(Rental Rental);
    Task<Rental?> UpdateRentalAsync(Rental Rental);
    Task<Rental?> UpdateRentalStatusAsync(Rental Rental);
    Task<bool> DeleteRentalAsync(int id);


  }
}
