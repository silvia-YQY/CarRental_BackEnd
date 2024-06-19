using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CarRentalPlatform.Models;
using CarRentalPlatform.DTOs;
using BCrypt.Net;

namespace CarRentalPlatform.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly CarRentalContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(CarRentalContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
      if (await _context.Users.AnyAsync(u => u.Email == userRegisterDto.Email))
        return BadRequest("Email is already in use.");

      var user = new User
      {
        Username = userRegisterDto.Username,
        Email = userRegisterDto.Email,
        isAdmin = userRegisterDto.isAdmin,
        Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password)
      };

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

      if (user == null)
      {
        return Unauthorized("Invalid email or password111.");
      }

      if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
      {
        return Unauthorized("Invalid email or password222.");
      }

      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtKey = _configuration["Jwt:Key"];
      if (jwtKey == null)
      {
        throw new InvalidOperationException("JWT key configuration 'Jwt:Key' is missing or null.");
      }
      var key = Encoding.UTF8.GetBytes(jwtKey);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
            // 如果有其他需要在令牌中存储的信息，可以继续添加
          }),
        Expires = DateTime.UtcNow.AddDays(1), // 设置过期时间
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);
      return Ok(new { Message = "Login successful", Token = tokenString });
    }
    private string GenerateJwtToken(User user)
    {
      var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

      // Ensure the key exists in configuration and handle null case
      var jwtKey = _configuration["Jwt:Key"];
      if (jwtKey == null)
      {
        throw new InvalidOperationException("JWT key configuration 'Jwt:Key' is missing or null.");
      }

      // Convert the key to bytes
      var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

      // Create SymmetricSecurityKey using the key bytes
      var key = new SymmetricSecurityKey(keyBytes);
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: _configuration["Jwt:Issuer"],
          audience: _configuration["Jwt:Issuer"],
          claims: claims,
          expires: DateTime.Now.AddMinutes(30),
          signingCredentials: creds);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
