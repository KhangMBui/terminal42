using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Terminal42.DTOs.Auth;
using Terminal42.Models;
using Terminal42.Repositories.Users;

namespace Terminal42.Services.Auth
{
  public class AuthService : IAuthService
  {
    private readonly IUserRepository _repo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository repo, IConfiguration config)
    {
      _repo = repo;
      _config = config;
    }

    public async Task<string> Register(RegisterRequest req)
    {
      // Check if the email existed
      var existing = await _repo.GetByEmailAsync(req.Email);
      if (existing != null)
      {
        throw new Exception("Email already exists");
      }

      // Hash the input password
      var hashed = BCrypt.Net.BCrypt.HashPassword(req.Password);

      // Create a new user
      var user = new User
      {
        Id = ObjectId.GenerateNewId().ToString(),
        Username = req.Username,
        Email = req.Email,
        PasswordHash = hashed
      };

      // Put it in repo
      await _repo.CreateAsync(user);

      // Return token
      return GenerateToken(user);
    }

    public async Task<string> Login(LoginRequest req)
    {
      // Fetch the user with req.Email
      var user = await _repo.GetByEmailAsync(req.Email);

      // User does not exist or wrong password
      if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
      {
        throw new Exception("Invalid Credentials");
      }

      // Return tokem
      return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtKey = _config["Jwt:Key"];
      if (string.IsNullOrEmpty(jwtKey))
        throw new Exception("JWT signing key is not configured.");
      var key = Encoding.UTF8.GetBytes(jwtKey);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity([
          new Claim(ClaimTypes.NameIdentifier, user.Id),
          new Claim(ClaimTypes.Name, user.Username)
        ]),
        Expires = DateTime.UtcNow.AddHours(2),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature
        )
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

  }
}