using Codeit.NetStdLibrary.Base.Abstractions.Desentralized;

namespace SchoolMngr.Services.Notifications.Test
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
