using CarRentalPlatform.Models;
using CarRentalPlatform.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalPlatform.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class CarsController : ControllerBase
  {
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
      _carService = carService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Car>>> GetCars()
    {
      var cars = await _carService.GetAllCarsAsync();
      return Ok(cars);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCar(int id)
    {
      var car = await _carService.GetCarByIdAsync(id);

      if (car == null)
      {
        return NotFound();
      }

      return Ok(car);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Car>> PostCar(Car car)
    {
      var createdCar = await _carService.CreateCarAsync(car);
      Console.WriteLine($"PostCar==={createdCar.Id}");
      return CreatedAtAction(nameof(GetCar), new { id = createdCar.Id }, createdCar);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCar(int id, Car car)
    {
      if (id != car.Id)
      {
        return BadRequest();
      }

      await _carService.UpdateCarAsync(car);
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
      await _carService.DeleteCarAsync(id);
      return NoContent();
    }

  }
}
