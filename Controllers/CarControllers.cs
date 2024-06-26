using AutoMapper;
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;
using CarRentalPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarRentalPlatform.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class CarsController : ControllerBase
  {
    private readonly ICarService _carService;
    private readonly IMapper _mapper;

    private readonly ILogger<CarsController> _logger;

    public CarsController(ICarService carService, IMapper mapper, ILogger<CarsController> logger)
    {
      _carService = carService;
      _mapper = mapper;
      _logger = logger;

    }

    [HttpGet("all")]
    public async Task<ActionResult<List<CarDto>>> GetCars()
    {
      try
      {
        var cars = await _carService.GetAllCarsAsync();
        var carDtos = _mapper.Map<List<CarDto>>(cars);

        return Ok(carDtos);
      }
      catch (Exception ex)
      {
        // 处理异常并记录日志
        _logger.LogError(ex, "An unexpected error occurred while getting all cars.");
        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
      }
    }

    [HttpGet("allByPage")]
    public async Task<ActionResult<PagedResult<CarDto>>> GetCars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
      try
      {
        var cars = await _carService.GetPagedCarsAsync(pageNumber, pageSize);
        var carDtos = _mapper.Map<List<CarDto>>(cars.Items);

        var pagedResult = new PagedResult<CarDto>
        {
          Items = carDtos,
          TotalCount = cars.TotalCount,
          PageNumber = pageNumber,
          PageSize = pageSize
        };

        return Ok(pagedResult);
      }
      catch (Exception ex)
      {
        // 处理异常并记录日志
        _logger.LogError(ex, "An unexpected error occurred while getting all cars.");
        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
      }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCar(int id)
    {
      var car = await _carService.GetCarByIdAsync(id);

      if (car == null)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"Car with Id {id} does not exist");
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
      try
      {


        var existingCar = await _carService.GetCarByIdAsync(id);

        if (id != car.Id)
        {
          return BadRequest("Id mismatch");
        }


        if (existingCar == null)
        {
          return StatusCode(StatusCodes.Status500InternalServerError, $"Car with Id {id} does not exist");
        }


        await _carService.UpdateCarAsync(car, existingCar);
        return Ok("Update successful");
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred" + ex);
      }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteCar(int id)
    {
      var car = await _carService.GetCarByIdAsync(id);
      if (car == null)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"Car with Id {id} does not exist");

      }
      await _carService.DeleteCarAsync(id);
      return Ok("Delete successful");
    }

  }
}
