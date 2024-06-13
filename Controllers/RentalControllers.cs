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
  public class RentalsController : ControllerBase
  {
    private readonly CarRentalContext _context;

    public RentalsController(CarRentalContext context)
    {
      _context = context;
    }

    [HttpGet("all")]
    public ActionResult<IEnumerable<Rental>> GetRentals()
    {
      return _context.Rentals.Include(r => r.Car)   // 包含关联的车辆信息
                         .Include(r => r.User)  // 包含关联的用户信息
                         .ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Rental> GetRental(int id)
    {
      var rental = _context.Rentals.Find(id);

      if (rental == null)
      {
        return NotFound();
      }

      return rental;
    }

    [HttpPost]
    public ActionResult<Rental> PostRental(Rental rental)
    {
      _context.Rentals.Add(rental);
      _context.SaveChanges();

      return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rental);
    }

    [HttpPut("{id}")]
    public IActionResult PutRental(int id, Rental rental)
    {
      if (id != rental.Id)
      {
        return BadRequest();
      }

      _context.Entry(rental).State = EntityState.Modified;
      _context.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRental(int id)
    {
      var rental = _context.Rentals.Find(id);
      if (rental == null)
      {
        return NotFound();
      }

      _context.Rentals.Remove(rental);
      _context.SaveChanges();

      return NoContent();
    }

  }
}
