using CarRentalPlatform.Models;
using CarRentalPlatform.Services;
using CarRentalPlatform.DTOs;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CarRentalPlatform.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly CarRentalContext _context;

    public UsersController(CarRentalContext context)
    {
      _context = context;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
      // .Select(u => new User { Id = u.Id, Username = u.Username, Email = u.Email, isAdmin = u.isAdmin })  // controll the paramer
      //.AsNoTracking()  // not track result
      var users = await _context.Users.AsNoTracking().ToListAsync();
      return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
      var user = await _context.Users.FindAsync(id);

      if (user == null)
      {
        return NotFound();
      }

      return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
      if (id != user.Id)
      {
        return BadRequest();
      }

      _context.Entry(user).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!_context.Users.Any(e => e.Id == id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user == null)
      {
        return NotFound();
      }

      _context.Users.Remove(user);
      await _context.SaveChangesAsync();

      return NoContent();
    }

  }
}
