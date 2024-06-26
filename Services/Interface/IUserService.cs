// Services/IUserService.cs
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;

namespace CarRentalPlatform.Services
{
  public interface IUserService
  {
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<PagedResult<User>> GetPagedUserAsync(int pageNumber, int pageSize);
    Task<User> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User User);
    Task UpdateUserAsync(User User);
    Task DeleteUserAsync(int id);
  }
}
