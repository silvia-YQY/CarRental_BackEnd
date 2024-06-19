using CarRentalPlatform.Models;
using CarRentalPlatform.Services;


using Microsoft.AspNetCore.Mvc;


namespace CarRentalPlatform.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _context;

    public UsersController(IUserService context)
    {
      _context = context;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
      // .Select(u => new User { Id = u.Id, Username = u.Username, Email = u.Email, isAdmin = u.isAdmin })  // controll the paramer
      //.AsNoTracking()  // not track result
      var users = await _context.GetAllUsersAsync();
      return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
      var user = await _context.GetUserByIdAsync(id);

      if (user == null)
      {
        return NotFound();
      }

      return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
      var createdUser = await _context.CreateUserAsync(user);
      return CreatedAtAction(nameof(GetUser), new { id = user.Id }, createdUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
      if (id != user.Id)
      {
        return BadRequest();
      }

      await _context.UpdateUserAsync(user);

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      await _context.DeleteUserAsync(id);
      return NoContent();
    }

  }
}
