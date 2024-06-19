using System.ComponentModel.DataAnnotations;
using CarRentalPlatform.Enums;

namespace CarRentalPlatform.DTOs
{

  public class RentalCreateDto
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "CarId is required")]
    public required int CarId { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public required int UserId { get; set; }

    [Required(ErrorMessage = "StartDate is required")]
    public required DateTime StartDate { get; set; }

    [Required(ErrorMessage = "StartDate is required")]
    public required DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Fee is required")]
    [Range(1, 10000, ErrorMessage = "Fee参数只能是大于1，小于10000")]
    public required decimal Fee { get; set; }
    public Status Status { get; set; }

    public string? CarMake { get; set; }
    public string? CarModel { get; set; }
    public string? UserName { get; set; }

    // public required CarDto Car { get; set; }
    // public required UserDto User { get; set; }
  }

  public class RentalStatusUpdateDto
  {
    [Required(ErrorMessage = "Status is required")]
    public Status Status { get; set; }
  }

}
