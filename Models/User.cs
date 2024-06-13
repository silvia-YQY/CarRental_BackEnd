using System.ComponentModel.DataAnnotations;

namespace CarRentalPlatform.Models;

public class User
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Password { get; set; }
  public bool User_type { get; set; }
  public string Email { get; set; }

}
