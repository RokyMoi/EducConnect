using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EduConnect.SignalIR
{
    public class PresenceHub(PresenceTracker tracker):Hub
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User == null)
            {
                throw new HubException("Cannot get user claim");
            }

            await tracker.UserConnected(Context.User?.FindFirst(ClaimTypes.Email)?.Value, Context.ConnectionId);
            
            await Clients.Others.SendAsync("UserIsOnline", Context.User?.FindFirst(ClaimTypes.Email)?.Value);
            var currentUsers = await tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }
        public async override Task OnDisconnectedAsync(Exception? exception)

           
        {
            if (Context.User == null)
            {
                throw new HubException("Cannot get user claim");
            }

            await tracker.UserDisconnected(Context.User?.FindFirst(ClaimTypes.Email)?.Value, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User?.FindFirst(ClaimTypes.Email)?.Value);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
