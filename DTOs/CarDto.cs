
namespace CarRentalPlatform.DTOs
{
  public class CarDto
  {
    public int Id { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public decimal Year { get; set; }
    public decimal Mileage { get; set; }
    public bool Available_Now { get; set; }
    public string Url { get; set; } = "";
    public decimal Price_Per_Day { get; set; }
  }

}
