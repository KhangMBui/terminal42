using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
using MongoDB.Driver;
using Terminal42.DTOs.Users;
using Terminal42.Models;

namespace Terminal42.Repositories.Users
{
  public class UserRepository : IUserRepository
  {
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<BsonDocument> _counters;

    public UserRepository(IMongoDatabase db)
    {
      _users = db.GetCollection<User>("Users");
      _counters = db.GetCollection<BsonDocument>("counters");
    }

    private async Task<string> GetNextUserIdAsync()
    {
      var filter = Builders<BsonDocument>.Filter.Eq("_id", "userId");
      var update = Builders<BsonDocument>.Update.Inc("seq", 1);
      var options = new FindOneAndUpdateOptions<BsonDocument>
      {
        IsUpsert = true,
        ReturnDocument = ReturnDocument.After
      };
      var result = await _counters.FindOneAndUpdateAsync(filter, update, options);
      var seq = result["seq"].AsInt32;
      return $"u{seq:D5}"; // e.g., u00001, u00002, etc.
    }

    public async Task<IEnumerable<User>> GetAllAsync() =>
      await _users.Find(_ => true).ToListAsync();


    public async Task<User> GetUserByIdAsync(string userId) => await _users.Find(u => u.UserId == userId).FirstOrDefaultAsync();

    public async Task<User> GetByEmailAsync(string email) =>
      await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

    public async Task CreateAsync(User user)
    {
      user.UserId = await GetNextUserIdAsync();
      await _users.InsertOneAsync(user);
    }

    public async Task<User> CreateUserAsync(CreateUserRequest req)
    {
      // Check if the email existed
      var existing = await _users.Find(u => u.Email == req.Email).FirstOrDefaultAsync();
      if (existing != null) throw new Exception("Email already existed");

      // Hash the password
      var hashed = BCrypt.Net.BCrypt.HashPassword(req.Password);

      // Create a new user
      var user = new User
      {
        Id = ObjectId.GenerateNewId().ToString(),
        Username = req.Username,
        Email = req.Email,
        PasswordHash = hashed
      };

      // Put it in the collection
      await CreateAsync(user);

      // Return the new user's UserId
      return user;
    }

    public async Task<User> UpdateUserAsync(string id, UpdateUserRequest req)
    {
      var existing = await _users.Find(u => u.UserId == id).FirstOrDefaultAsync();
      if (existing == null) throw new Exception("User not found");

      // Update username
      var update = Builders<User>.Update.Set(u => u.Username, req.Username);

      // Only update password if provided in dto
      if (!string.IsNullOrEmpty(req.Password))
      {
        var hashed = BCrypt.Net.BCrypt.HashPassword(req.Password);
        update = update.Set(u => u.PasswordHash, hashed);
      }

      await _users.UpdateOneAsync(u => u.UserId == id, update);
      
      return await _users.Find(u => u.UserId == id).FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
      var existing = await _users.Find(u => u.UserId == id).FirstOrDefaultAsync();
      if (existing == null) throw new Exception("User not found");

      var response = await _users.DeleteOneAsync(u => u.UserId == id);
      return response.DeletedCount > 0;
    }
  }
}