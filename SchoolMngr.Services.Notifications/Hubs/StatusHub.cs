using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fitnner.Trainers.Notifications.Hubs
{
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
