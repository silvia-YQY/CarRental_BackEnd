using System.ComponentModel.DataAnnotations;

namespace CarRentalPlatform.Models;

public class User
{
  public int Id { get; set; }

  [Required(ErrorMessage = "Username is required")]
  public string Username { get; set; } = "";

  [Required(ErrorMessage = "Password is required")]
  public string Password { get; set; } = "";
  public bool isAdmin { get; set; }

  [Required(ErrorMessage = "Email is required")]
  public string Email { get; set; } = "";

  // public ICollection<Rental>? Rentals { get; set; }
  public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

}
