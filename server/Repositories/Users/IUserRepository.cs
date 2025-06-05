using Terminal42.Models;

namespace Terminal42.Repositories.Users
{
  public interface IUserRepository
  {
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByEmailAsync(string email);
    Task CreateAsync(User user);
  }
}