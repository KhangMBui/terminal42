using Microsoft.AspNetCore.Mvc;
using Terminal42.DTOs.Users;
using Terminal42.Models;
using Terminal42.Services.Users;

namespace Terminal42.Controllers
{
  [ApiController]
  [Route("api/users")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _user;

    public UserController(IUserService user)
    {
      _user = user;
    }

    [HttpGet("")]
    public async Task<IEnumerable<User>> GetAllUsers()
    {
      var users = await _user.GetAllUsersAsync();
      return users;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<User>> GetUserByUserId(string userId)
    {
      var user = await _user.GetUserByIdAsync(userId);
      if (user == null) return NotFound();
      return user;
    }

    [HttpPost("")]
    public async Task<User> CreateUser(CreateUserRequest createUserRequestDto)
    {
      return await _user.CreateUserAsync(createUserRequestDto);
    }

    [HttpPut("{userId}")]
    public async Task<User> UpdateUser(string userId, UpdateUserRequest updateUserRequestDto)
    {
      return await _user.UpdateUserAsync(userId, updateUserRequestDto);
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<string>> DeleteUserById(string userId)
    {
      bool isDeleted = await _user.DeleteUserAsync(userId);
      if (isDeleted)
          return Ok($"Deleted successfully user with id {userId}");
      else
          return NotFound("Deleted unsuccessfully");
    }
  }
}