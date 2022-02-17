using Microsoft.AspNetCore.SignalR;

namespace SchoolMngr.Services.Notifications.Hubs;

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
