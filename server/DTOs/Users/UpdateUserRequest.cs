namespace Terminal42.DTOs.Users
{
  public class UpdateUserRequest
  {
    public required string Username { get; set; }
    // public required string Email { get; set; }
    public required string Password { get; set; }
  }
}