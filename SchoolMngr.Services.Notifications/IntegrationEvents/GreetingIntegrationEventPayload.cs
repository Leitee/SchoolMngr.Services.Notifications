namespace SchoolMngr.Services.Notifications.IntegrationEvents
{
    using Codeit.NetStdLibrary.Base.Abstractions.Desentralized;
    public class GreetingIntegrationEventPayload : IntegrationEventPayload
    {
        public string SenderName { get; set; }
        public string Greeting { get; private set; }

        public GreetingIntegrationEventPayload(string senderName, string greeting)
        {
            Greeting = greeting;
            SenderName = senderName;
        }
    }
}
