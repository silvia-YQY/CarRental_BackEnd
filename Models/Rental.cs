using System.ComponentModel.DataAnnotations;
using CarRentalPlatform.Enums;

namespace CarRentalPlatform.Models


{

  public class Rental
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "CarId is required")]
    public int CarId { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "StartDate is required")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Fee is required")]
    public decimal Fee { get; set; }
    public Status Status { get; set; }

    public required Car Car { get; set; }
    public required User User { get; set; }

  }

}
