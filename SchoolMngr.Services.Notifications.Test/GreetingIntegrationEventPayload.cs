using Codeit.NetStdLibrary.Base.Abstractions.Desentralized;

namespace Fitnner.Trainers.Notifications.FunctionalTest
{
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
