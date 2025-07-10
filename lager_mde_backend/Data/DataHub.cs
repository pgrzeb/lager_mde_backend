using Microsoft.AspNetCore.SignalR;

public class DataHub : Hub
{
    public async Task SendUpdate(string userId)
    {
        await Clients.User(userId).SendAsync("ReceiveDataUpdate");
    }
}
