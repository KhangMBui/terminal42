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
      return await _repo.GetUserByIdAsync(userId);
    }
    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
      return await _repo.CreateUserAsync(request);
    }
    public async Task<User> UpdateUserAsync(string id, UpdateUserRequest request)
    {
      return await _repo.UpdateUserAsync(id, request);
    }
    public async Task<bool> DeleteUserAsync(string id)
    {
      return await _repo.DeleteUserAsync(id);
    }
    
  }
}