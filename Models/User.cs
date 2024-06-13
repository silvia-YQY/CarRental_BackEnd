using System.ComponentModel.DataAnnotations;

namespace CarRentalPlatform.Models;

public class User
{
  public int Id { get; set; }
  public string Username { get; set; }
  public string Password { get; set; }
  public bool isAdmin { get; set; }
  public string Email { get; set; }

  public ICollection<Rental> Rentals { get; set; }

}
