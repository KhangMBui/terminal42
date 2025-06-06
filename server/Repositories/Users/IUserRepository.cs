using Terminal42.DTOs.Users;
using Terminal42.Models;

namespace Terminal42.Repositories.Users
{
  public interface IUserRepository
  {
    Task<User> CreateUserAsync(CreateUserRequest req);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByUserIdAsync(string userId);
    Task<User> GetByEmailAsync(string email);
    Task CreateAsync(User user);
    Task<bool> DeleteUserAsync(string id);
  }
}