using Codeit.Enterprise.Base.Desentralized.IntegrationEvent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SchoolMngr.Services.Notifications.Hubs;
using SchoolMngr.Services.Notifications.Services.Interfaces;

namespace SchoolMngr.Services.Notifications.Services;

public class ClientNotificationService : IClientNotificationService
{
    private readonly ILogger<ClientNotificationService> _logger;
    private readonly IHubContext<NotificationsHub> _hubContext;

    public ClientNotificationService(IHubContext<NotificationsHub> hubContext, ILoggerFactory loggerFactory)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<ClientNotificationService>();
    }

    public async Task SendNotificationAsync(ClientNotificationIntegrationEventPayload payload)
    {
        _logger.LogDebug($"Notification to client: {payload.WSConnectionId} - {payload.Payload}");
        await _hubContext.Clients.Client(payload.WSConnectionId).SendAsync(payload.CallBackMethod, payload.Payload);
    }
}
