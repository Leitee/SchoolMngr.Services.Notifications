using SchoolMngr.Services.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Codeit.NetStdLibrary.Base.Abstractions.Desentralized;
using System;
using System.Threading.Tasks;

namespace SchoolMngr.Services.Notifications
{
    public class ProccessGreetingIntegrationEventHandler : IIntegrationEventHandler<GreetingIntegrationEventPayload>
    {
        private readonly ILogger<ProccessGreetingIntegrationEventHandler> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public ProccessGreetingIntegrationEventHandler(IHubContext<NotificationsHub> hubContext, ILoggerFactory loggerFactory)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<ProccessGreetingIntegrationEventHandler>();
        }

        public async Task Handle(GreetingIntegrationEventPayload @event)
        {
            _logger.LogDebug($"Payload message says: {@event.Greeting}", @event);
            await _hubContext.Clients.Group(@event.SenderName).SendAsync("GreetingSent", @event);
        }
    }
}
