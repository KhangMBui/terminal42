using Microsoft.AspNetCore.Mvc;
using Terminal42.DTOs.Auth;
using Terminal42.Services.Auth;

namespace Terminal42.Controllers
{
  [ApiController]
  [Route("api/auth")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
      _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
      var token = await _auth.Register(req);
      return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
      var token = await _auth.Login(req);
      return Ok(new { token });
    }
  }
}