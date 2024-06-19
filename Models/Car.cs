using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalPlatform.Models;

public class Car
{
  public int Id { get; set; }

  [Required(ErrorMessage = "Make is required")]
  public string Make { get; set; } = "";

  [Required(ErrorMessage = "Model is required")]
  public string Model { get; set; } = "";

  [Required(ErrorMessage = "Year is required")]
  public decimal Year { get; set; }

  [Required(ErrorMessage = "Mileage is required")]
  public decimal Mileage { get; set; }

  public bool Available_Now { get; set; } = false;

  public string Url { get; set; } = "";

  [Required(ErrorMessage = "Price_Per_Day is required")]
  public decimal Price_Per_Day { get; set; }

  // public ICollection<Rental>? Rentals { get; set; }
  public ICollection<Rental> Rentals { get; set; } = new List<Rental>(); // 初始化属性

}
