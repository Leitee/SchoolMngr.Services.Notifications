
namespace SchoolMngr.Services.Notifications.Hubs
{
    using Codeit.NetStdLibrary.Base.DomainModel;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Threading.Tasks;

    //[Authorize]
    public class NotificationsHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnDisconnectedAsync(ex);
        }

        public async Task SendNotificationAsync(string connectionId, string callbackName, ClientNotificationResult payload)
        {
            await Clients.Client(connectionId).SendAsync(callbackName, payload);
        }
    }
}
