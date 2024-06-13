using System.ComponentModel.DataAnnotations;

namespace CarRentalPlatform.Models;

public class Car
{
  public int Id { get; set; }
  public string Make { get; set; }
  public string Model { get; set; }
  public decimal Year { get; set; }
  public decimal Mileage { get; set; }
  public bool Available_Now { get; set; }
  public string Url { get; set; }
  public decimal Price_Per_Day { get; set; }

  public ICollection<Rental> Rentals { get; set; }
}
