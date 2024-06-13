using CarRentalPlatform.Models;
using CarRentalPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
      return _context.Users.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id)
    {
      var user = _context.Users.Find(id);

      if (user == null)
      {
        return NotFound();
      }

      return user;
    }

    [HttpPost]
    public ActionResult<User> PostUser(User user)
    {
      _context.Users.Add(user);
      _context.SaveChanges();

      return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public IActionResult PutUser(int id, User user)
    {
      if (id != user.Id)
      {
        return BadRequest();
      }

      _context.Entry(user).State = EntityState.Modified;
      _context.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        return NotFound();
      }

      _context.Users.Remove(user);
      _context.SaveChanges();

      return NoContent();
    }

  }
}
