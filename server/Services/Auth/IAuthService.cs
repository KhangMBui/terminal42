using Terminal42.DTOs.Auth;

namespace Terminal42.Services.Auth
{
  public interface IAuthService
  {
    Task<string> Register(RegisterRequest request);
    Task<string> Login(LoginRequest request);
  }
}