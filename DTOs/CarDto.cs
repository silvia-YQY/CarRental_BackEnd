
using System.ComponentModel.DataAnnotations;

namespace CarRentalPlatform.DTOs
{
  public class CarDto
  {
    public int Id { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }

    [Range(1900, 2024, ErrorMessage = "Year must between 1990 to 2024")]
    public required decimal Year { get; set; }

    [Range(1, 10000, ErrorMessage = "Mileage must between 1 to 10000")]
    public required decimal Mileage { get; set; }
    public bool Available_Now { get; set; }
    public string Url { get; set; } = "";

    [Required(ErrorMessage = "Price_Per_Day is required")]
    [Range(1, 10000, ErrorMessage = "Year must between 1 to 10000")]
    public decimal Price_Per_Day { get; set; }

    public List<RentalCreateDto> Rentals { get; set; } = new List<RentalCreateDto>();
  }

}
