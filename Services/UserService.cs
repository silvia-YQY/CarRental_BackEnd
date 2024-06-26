// Services/UserService.cs
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPlatform.Services
{
  public class UserService : IUserService
  {
    private readonly CarRentalContext _context;

    public UserService(CarRentalContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
      return await _context.Users.ToListAsync();
    }

    public async Task<PagedResult<User>> GetPagedUserAsync(int pageNumber, int pageSize)
    {
      var query = _context.Users.AsQueryable();

      var totalCount = await query.CountAsync();
      var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

      return new PagedResult<User>
      {
        Items = items,
        TotalCount = totalCount,
        PageNumber = pageNumber,
        PageSize = pageSize
      };
    }
    public async Task<User> GetUserByIdAsync(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user == null)
      {
        throw new KeyNotFoundException($"User with ID {id} not found.");
      }
      return user;

    }

    public async Task<User> CreateUserAsync(User User)
    {
      _context.Users.Add(User);
      await _context.SaveChangesAsync();
      Console.WriteLine($"CreateUserAsync==={User.Id}");

      return User;
    }

    public async Task UpdateUserAsync(User User)
    {
      _context.Entry(User).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
      var User = await _context.Users.FindAsync(id);
      if (User != null)
      {
        _context.Users.Remove(User);
        await _context.SaveChangesAsync();
      }
    }
  }
}
