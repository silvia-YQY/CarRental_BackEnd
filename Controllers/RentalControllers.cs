using AutoMapper;
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;
using CarRentalPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // 引用 DbUpdateException
using Microsoft.AspNetCore.Authorization;


namespace CarRentalPlatform.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class RentalsController : ControllerBase
  {
    private readonly IRentalService _rentalService;
    private readonly IMapper _mapper;
    private readonly ICarService _carService;


    private readonly ILogger<RentalsController> _logger; // 添加 ILogger 字段

    public RentalsController(IRentalService rentalService, IMapper mapper, ICarService carService, ILogger<RentalsController> logger)
    {
      _rentalService = rentalService;
      _logger = logger;
      _mapper = mapper;
      _carService = carService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<RentalCreateDto>>> GetRentals()
    {
      var rentals = await _rentalService.GetAllRentalsAsync();
      var rentalsDtos = _mapper.Map<IEnumerable<RentalCreateDto>>(rentals);

      return Ok(rentalsDtos);
    }

    [HttpGet("allByPage")]
    public async Task<ActionResult<PagedResult<RentalCreateDto>>> GetRentals([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
      try
      {
        var rentals = await _rentalService.GetPagedRentalAsync(pageNumber, pageSize);
        var RentalCreateDtos = _mapper.Map<List<RentalCreateDto>>(rentals.Items);

        var pagedResult = new PagedResult<RentalCreateDto>
        {
          Items = RentalCreateDtos,
          TotalCount = rentals.TotalCount,
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
    public async Task<ActionResult<Rental>> GetRental(int id)
    {
      var rental = await _rentalService.GetRentalByIdAsync(id);

      if (rental == null)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"Car with Id {id} does not exist");
      }
      var rentalDto = _mapper.Map<RentalCreateDto>(rental);
      return Ok(rentalDto);
    }

    [HttpPost]
    public async Task<ActionResult<string>> PostRental([FromBody] RentalCreateDto rentalDto)
    {
      try
      {

        // Map RentalCreateDto to Rental
        var rental = _mapper.Map<Rental>(rentalDto);

        var createdRental = await _rentalService.CreateRentalAsync(rental);

        var createdRentalDto = _mapper.Map<RentalCreateDto>(createdRental);

        return CreatedAtAction(nameof(GetRental), new { id = createdRental.Id }, createdRental);

        // return Content(json, "application/json");
      }
      catch (DbUpdateException ex)
      {
        // 捕获数据库更新异常
        _logger.LogError("DbUpdateException" + ex);
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create rental" + ex);
      }
      catch (Exception ex)
      {
        // 捕获其他异常
        _logger.LogError("An unexpected error occurred" + ex);
        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred" + ex);
      }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutRental(int id, RentalCreateDto rentalDto)
    {
      try
      {
        var existingRental = await _rentalService.GetRentalByIdAsync(rentalDto.Id);
        if (existingRental == null)
        {

          return StatusCode(StatusCodes.Status500InternalServerError, $"Rental with ID {rentalDto.Id} not found.");
        }

        if (id != rentalDto.Id)
        {
          _logger.LogError("Failed to update rental, Id mismatch");
          return BadRequest("Id mismatch");
        }

        // Check if the CarId exists in the database
        var existingCar = await _carService.GetCarByIdAsync(rentalDto.CarId);
        if (existingCar == null)
        {
          _logger.LogError($"Car with Id {rentalDto.CarId} does not exist");
          return StatusCode(StatusCodes.Status500InternalServerError, $"Car with Id {rentalDto.CarId} does not exist");
        }
        // Use AutoMapper to map rentalDto properties to existingRental
        _mapper.Map(rentalDto, existingRental);

        await _rentalService.UpdateRentalAsync(existingRental);

        var updatedRentalDto = _mapper.Map<RentalCreateDto>(rentalDto);

        return Ok("Update successful");
      }
      catch (DbUpdateException ex)
      {
        // Handle database update exception
        _logger.LogError(ex, "Failed to update rental" + ex);
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update rental" + ex);
      }
      catch (Exception ex)
      {
        // Handle other exceptions
        _logger.LogError(ex, "An unexpected error occurred" + ex);
        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred" + ex);
      }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRental(int id)
    {

      var existingRental = await _rentalService.GetRentalByIdAsync(id);
      if (existingRental != null)
      {
        await _rentalService.DeleteRentalAsync(id);
        return Ok("Delete successful");
      }
      else
      {
        _logger.LogError("Failed to delete rental, Id mismatch");
        return BadRequest("Id mismatch");
      }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("status/{id}")]
    public async Task<IActionResult> UpdateRentalStatus(int id, [FromBody] RentalStatusUpdateDto statusUpdateDto)
    {
      try
      {
        // 查找现有的Rental
        var rental = await _rentalService.GetRentalByIdAsync(id);
        if (rental == null)
        {
          _logger.LogError($"Rental with id {id} not found");
          return NotFound($"Rental with id {id} not found");
        }

        // 更新状态
        rental.Status = statusUpdateDto.Status;

        // 保存更改
        await _rentalService.UpdateRentalStatusAsync(rental);

        return Ok("Update successful");
      }
      catch (DbUpdateException ex)
      {
        _logger.LogError(ex, "Failed to update rental status");
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update rental status");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An unexpected error occurred");
        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
      }
    }

  }
}
