/// <summary>
/// 
/// </summary>
namespace Fitnner.Trainers.Notifications
{
    using Pandora.NetStdLibrary.Base.Abstractions;
    using Pandora.NetStdLibrary.Base.Abstractions.Desentralized;
    using Pandora.NetStdLibrary.Base.Constants;
    using System;

    public class CrudNotificationIntegrationEventPayload : IntegrationEventPayload, IClientNotification
    {
        public Guid ClientID { get; private set; }
        public string Message { get; private set; }
        public CrudOperationType ClientOperation { get; set; }

        public CrudNotificationIntegrationEventPayload(Guid clientId, CrudOperationType operationType, string message) : base()
        {
            ClientID = clientId;
            ClientOperation = operationType;
            Message = message;
        }
    }
}
