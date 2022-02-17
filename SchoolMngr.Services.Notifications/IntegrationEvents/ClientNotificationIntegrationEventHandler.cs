
namespace SchoolMngr.Services.Notifications.IntegrationEvents
{
    using Codeit.Enterprise.Base.Abstractions.Desentralized;
    using Codeit.Enterprise.Base.Desentralized.IntegrationEvent;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using SchoolMngr.Services.Notifications.Hubs;
    using System;
    using System.Threading.Tasks;

    public class ClientNotificationIntegrationEventHandler : IIntegrationEventHandler<ClientNotificationIntegrationEventPayload>
    {
        private readonly ILogger<ClientNotificationIntegrationEventHandler> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public ClientNotificationIntegrationEventHandler(IHubContext<NotificationsHub> hubContext, ILoggerFactory loggerFactory)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<ClientNotificationIntegrationEventHandler>();
        }

        public async Task Handle(ClientNotificationIntegrationEventPayload @event)
        {
            _logger.LogDebug($"Notification to client: {@event.WSConnectionId} - {@event.Payload}");
            await _hubContext.Clients.Client(@event.WSConnectionId).SendAsync(@event.CallBackMethod, @event.Payload);
        }
    }
}
