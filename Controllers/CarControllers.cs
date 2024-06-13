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
  public class CarsController : ControllerBase
  {
    private readonly CarRentalContext _context;

    public CarsController(CarRentalContext context)
    {
      _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Car>> GetCars()
    {
      return _context.Cars.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Car> GetCar(int id)
    {
      var car = _context.Cars.Find(id);

      if (car == null)
      {
        return NotFound();
      }

      return car;
    }

    [HttpPost]
    public ActionResult<Car> PostCar(Car car)
    {
      _context.Cars.Add(car);
      _context.SaveChanges();

      return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
    }

    [HttpPut("{id}")]
    public IActionResult PutCar(int id, Car car)
    {
      Console.WriteLine("id=>", id);
      Console.WriteLine("car id=>", car.Id);
      if (id != car.Id)
      {
        return BadRequest();
      }

      _context.Entry(car).State = EntityState.Modified;
      _context.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCar(int id)
    {
      var car = _context.Cars.Find(id);
      if (car == null)
      {
        return NotFound();
      }

      _context.Cars.Remove(car);
      _context.SaveChanges();

      return NoContent();
    }

  }
}
