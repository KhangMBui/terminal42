using Microsoft.AspNetCore.SignalR;

namespace Terminal42.Hubs;

public class ChatHub : Hub
{
  // Send message to all clients
  public async Task SendMessage(string user, string message)
  {
    await Clients.All.SendAsync("ReceiveMessage", user, message);
  }
}