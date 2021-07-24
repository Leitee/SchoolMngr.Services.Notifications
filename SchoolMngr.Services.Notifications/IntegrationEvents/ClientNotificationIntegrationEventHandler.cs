using SchoolMngr.Services.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Codeit.NetStdLibrary.Base.Abstractions.Desentralized;
using Codeit.NetStdLibrary.Base.Common;
using System;
using System.Threading.Tasks;

namespace SchoolMngr.Services.Notifications
{
    public class ClientNotificationIntegrationEventHandler : IIntegrationEventHandler<CrudNotificationIntegrationEventPayload>
    {
        private readonly ILogger<ClientNotificationIntegrationEventHandler> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public ClientNotificationIntegrationEventHandler(IHubContext<NotificationsHub> hubContext, ILoggerFactory loggerFactory)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<ClientNotificationIntegrationEventHandler>();
        }

        public async Task Handle(CrudNotificationIntegrationEventPayload @event)
        {
            _logger.LogDebug($"Notification to client: {@event.ClientID} of type {@event.ClientOperation.GetDescription()}");
            await _hubContext.Clients.Group(@event.ClientID.ToString()).SendAsync("GreetingSent", @event);
        }
    }
}
