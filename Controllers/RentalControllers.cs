using System.Text.Json;
using System.Text.Json.Serialization;
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;
using CarRentalPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // 引用 DbUpdateException


namespace CarRentalPlatform.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class RentalsController : ControllerBase
  {
    private readonly IRentalService _rentalService;
    private readonly ILogger<RentalsController> _logger; // 添加 ILogger 字段

    public RentalsController(IRentalService rentalService, ILogger<RentalsController> logger)
    {
      _rentalService = rentalService;
      _logger = logger;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
    {
      var rentals = await _rentalService.GetAllRentalsAsync();
      return Ok(rentals);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Rental>> GetRental(int id)
    {
      var rental = await _rentalService.GetRentalByIdAsync(id);

      if (rental == null)
      {
        return NotFound();
      }

      return Ok(rental);
    }

    [HttpPost]
    public async Task<ActionResult<Rental>> PostRental(RentalCreateDto rentalDto)
    {
      try
      {


        var createdRental = await _rentalService.CreateRentalAsync(rentalDto);

        var options = new JsonSerializerOptions
        {
          ReferenceHandler = ReferenceHandler.Preserve,
        };

        var json = JsonSerializer.Serialize(createdRental, options);


        return Content(json, "application/json");
      }
      catch (DbUpdateException ex)
      {
        // 捕获数据库更新异常
        _logger.LogError(ex, "Failed to create rental");
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create rental");
      }
      catch (Exception ex)
      {
        // 捕获其他异常
        _logger.LogError(ex, "An unexpected error occurred");
        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
      }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutRental(int id, RentalCreateDto rentalDto)
    {
      if (id != rentalDto.Id)
      {
        return BadRequest();
      }

      await _rentalService.UpdateRentalAsync(rentalDto);
      return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRental(int id)
    {

      await _rentalService.DeleteRentalAsync(id);
      return NoContent();
    }

  }
}
