using Terminal42.DTOs.Users;
using Terminal42.Models;

namespace Terminal42.Services.Users
{
  public interface IUserService
  {
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(string id);
    Task<User> CreateUserAsync(CreateUserRequest request);
    Task<User> UpdateUserAsync(string id, UpdateUserRequest request);
    Task<bool> DeleteUserAsync(string id);
  }
}