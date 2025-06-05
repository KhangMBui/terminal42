using MongoDB.Driver;
using Terminal42.Models;

namespace Terminal42.Repositories.Users
{
  public class UserRepository : IUserRepository
  {
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoDatabase db)
    {
      _users = db.GetCollection<User>("Users");
    }

    public async Task<IEnumerable<User>> GetAllAsync() =>
      await _users.Find(_ => true).ToListAsync();

    public async Task<User> GetByEmailAsync(string email) =>
      await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

    public async Task CreateAsync(User user) =>
      await _users.InsertOneAsync(user);
  }
}