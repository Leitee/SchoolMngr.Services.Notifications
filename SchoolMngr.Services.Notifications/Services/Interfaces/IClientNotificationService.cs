using Codeit.Enterprise.Base.Desentralized.IntegrationEvent;

namespace SchoolMngr.Services.Notifications.Services.Interfaces;

public interface IClientNotificationService
{
    Task SendNotificationAsync(ClientNotificationIntegrationEventPayload payload);
}
