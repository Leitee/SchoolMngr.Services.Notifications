
namespace SchoolMngr.Services.Notifications.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class StatusHub : Hub
    {

        public async Task BypassMessage(string message)
        {
            await Clients
                .Caller
                .SendAsync("OnBypassMsg", message);
        }

        public async Task StatusReport()
        {
            await Clients
                .Caller
                .SendAsync("OnStatusReport", "Healthy");
        }
    }
}
