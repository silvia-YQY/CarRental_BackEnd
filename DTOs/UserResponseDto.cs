namespace CarRentalPlatform.DTOs
{
  public class UserResponseDto
  {
    public required string Username { get; set; } = "";
    public required string Email { get; set; } = "";
    public required bool isAdmin { get; set; }

  }
}
