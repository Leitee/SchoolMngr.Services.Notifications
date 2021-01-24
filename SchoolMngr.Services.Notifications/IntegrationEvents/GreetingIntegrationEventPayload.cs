﻿/// <summary>
/// 
/// </summary>
namespace Fitnner.Trainers.Notifications
{
    using Pandora.NetStdLibrary.Base.Abstractions.Desentralized;
    public class GreetingIntegrationEventPayload : IntegrationEventPayload
    {
        public string SenderName { get; set; }
        public string Greeting { get; private set; }

        public GreetingIntegrationEventPayload(string senderName, string greeting) : base()
        {
            Greeting = greeting;
            SenderName = senderName;
        }
    }
}
