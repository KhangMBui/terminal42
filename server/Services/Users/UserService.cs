using Terminal42.DTOs.Users;
using Terminal42.Models;
using Terminal42.Repositories.Users;

namespace Terminal42.Services.Users
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
      _repo = repo;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
      return await _repo.GetAllAsync();
    }
    public async Task<User?> GetUserByIdAsync(string userId)
    {
      return await _repo.GetByUserIdAsync(userId);
    }
    // public Task<User> CreateUserAsync(CreateUserRequest request)
    // {
      
    // }
    // public Task<User> UpdateUserAsync(string id, UpdateUserRequest request)
    // {
      
    // }
    // public Task<bool> DeleteUserAsync(string id)
    // {
      
    // }
    
  }
}