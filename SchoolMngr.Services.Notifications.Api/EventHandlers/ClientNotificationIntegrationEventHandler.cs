
using SchoolMngr.Services.Notifications.Services.Interfaces;

namespace SchoolMngr.Services.Notifications.IntegrationEvents;

public class ClientNotificationIntegrationEventHandler : IIntegrationEventHandler<ClientNotificationIntegrationEventPayload>
{
    private readonly IClientNotificationService _clientNotificationService;

    public ClientNotificationIntegrationEventHandler(IClientNotificationService clientNotificationService)
    {
        _clientNotificationService = clientNotificationService;
    }

    public async Task Handle(ClientNotificationIntegrationEventPayload @event)
    {
        await _clientNotificationService.SendNotificationAsync(@event);
    }
}
