using System.ComponentModel.DataAnnotations;
using CarRentalPlatform.Enums;


namespace CarRentalPlatform.DTOs
{
  public class CarDto
  {
    public int Id { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public bool AvailableNow { get; set; }
    public string Url { get; set; }
    public decimal PricePerDay { get; set; }
  }

  public class UserDto
  {
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public bool IsAdmin { get; set; }
  }
  public class RentalCreateDto
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "CarId is required")]
    public int CarId { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Fee { get; set; }
    public Status Status { get; set; }

    public required CarDto Car { get; set; }
    public required UserDto User { get; set; }
  }

}
