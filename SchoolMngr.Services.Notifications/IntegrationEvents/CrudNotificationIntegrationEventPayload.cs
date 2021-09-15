﻿/// <summary>
/// 
/// </summary>
namespace SchoolMngr.Services.Notifications.IntegrationEvents
{
    using Codeit.NetStdLibrary.Base.Abstractions.BusinessLogic;
    using Codeit.NetStdLibrary.Base.Abstractions.Desentralized;
    using System;

    public class CrudNotificationIntegrationEventPayload : IntegrationEventPayload
    {
        public Guid ClientID { get; private set; }
        public string Message { get; private set; }
        public CrudOperationEnum ClientOperation { get; set; }

        public CrudNotificationIntegrationEventPayload(Guid clientId, CrudOperationEnum operationType, string message) : base()
        {
            ClientID = clientId;
            ClientOperation = operationType;
            Message = message;
        }
    }
}
