using Microsoft.AspNetCore.Mvc;
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
  }
}