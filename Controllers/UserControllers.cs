using AutoMapper;
using CarRentalPlatform.DTOs;
using CarRentalPlatform.Models;
using CarRentalPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarRentalPlatform.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _context;

    private readonly IMapper _mapper;

    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService context, IMapper mapper, ILogger<UsersController> logger)
    {
      _context = context;
      _mapper = mapper;
      _logger = logger;

    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
      // .Select(u => new User { Id = u.Id, Username = u.Username, Email = u.Email, isAdmin = u.isAdmin })  // controll the paramer
      //.AsNoTracking()  // not track result
      var users = await _context.GetAllUsersAsync();
      var userDtos = _mapper.Map<List<UserDto>>(users);

      return Ok(userDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
      var user = await _context.GetUserByIdAsync(id);

      if (user == null)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"user with Id {id} does not exist");
      }
      var userDto = _mapper.Map<UserDto>(user);

      return Ok(userDto);
    }

    [HttpGet("allByPage")]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
      try
      {
        var users = await _context.GetPagedUserAsync(pageNumber, pageSize);
        var UserDtos = _mapper.Map<List<UserDto>>(users.Items);

        var pagedResult = new PagedResult<UserDto>
        {
          Items = UserDtos,
          TotalCount = users.TotalCount,
          PageNumber = pageNumber,
          PageSize = pageSize
        };

        return Ok(pagedResult);
      }
      catch (Exception ex)
      {
        // 处理异常并记录日志
        _logger.LogError(ex, "An unexpected error occurred while getting all cars.");
        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
      }
    }


    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
      var createdUser = await _context.CreateUserAsync(user);
      var createdUserDto = _mapper.Map<UserDto>(createdUser);

      return CreatedAtAction(nameof(GetUser), new { id = user.Id }, createdUserDto);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
      if (id != user.Id)
      {
        return BadRequest("Id mismatch");
      }

      await _context.UpdateUserAsync(user);

      return Ok("Update successful");
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      var User = await _context.GetUserByIdAsync(id);
      if (User == null)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"User with Id {id} does not exist");

      }
      await _context.DeleteUserAsync(id);
      return Ok("Delete successful");
    }

  }
}
