using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Terminal42.Models
{
  public class User
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    [BsonElement("userId")]
    public string? UserId { get; set; }

    [BsonElement("username")]
    public required string Username { get; set; }

    [BsonElement("email")]
    public required string Email { get; set; }

    [BsonElement("password")]
    public required string PasswordHash { get; set; }
        
  }
}
