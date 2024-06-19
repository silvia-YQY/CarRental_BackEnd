using AutoMapper;
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;
using CarRentalPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalPlatform.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class CarsController : ControllerBase
  {
    private readonly ICarService _carService;
    private readonly IMapper _mapper;

    public CarsController(ICarService carService, IMapper mapper)
    {
      _carService = carService;
      _mapper = mapper;
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<Car>>> GetCars()
    {
      var cars = await _carService.GetAllCarsAsync();
      var carDtos = _mapper.Map<List<CarDto>>(cars);
      return Ok(carDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCar(int id)
    {
      var car = await _carService.GetCarByIdAsync(id);

      if (car == null)
      {
        return NotFound();
      }
      CarDto carDto = _mapper.Map<CarDto>(car);

      return Ok(carDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<Car>> PostCar(Car car)
    {
      var createdCar = await _carService.CreateCarAsync(car);
      CarDto carDto = _mapper.Map<CarDto>(createdCar);

      return CreatedAtAction(nameof(GetCar), new { id = carDto.Id }, carDto);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminPolicy")]
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
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteCar(int id)
    {
      await _carService.DeleteCarAsync(id);
      return NoContent();
    }

  }
}
