namespace SchoolMngr.Services.Notifications.FunctionalTest
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Codeit.NetStdLibrary.Base.Abstractions.Desentralized;
    using Codeit.NetStdLibrary.Base.Desentralized.EventBus;
    using Codeit.NetStdLibrary.Base.Desentralized.EventBusRabbitMQ;
    using RabbitMQ.Client;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        // Connection String for the namespace can be obtained from the Azure portal under the 
        // 'Shared Access policies' section.
        const string BROKER_NAME = "test_event_bus";

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILoggerFactory _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly int _retryCount;
        private static IEventBus _queueClient;

        public Program()
        {
            _retryCount = 3;
            var _connection = new ConnectionFactory()
            {
                HostName = "192.168.1.10",
                DispatchConsumersAsync = true,
                UserName = "guest",
                Password = "guest"
            };

            _logger = NullLoggerFactory.Instance;
            _persistentConnection = new DefaultRabbitMQPersistentConnection(_connection, _logger, _retryCount);
            _subsManager = new InMemoryEventBusSubscriptionsManager();
            _queueClient = new EventBusRabbitMQ(_persistentConnection, _logger, null, _subsManager, BROKER_NAME, _retryCount);
        }

        public static void Main(string[] args)
        {

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send messages.
            new Program();
            for (int i = 1; i < 100; i++)
            {
                _queueClient.Publish(new GreetingIntegrationEventPayload("Hayek", $"Dummy message number: {i}"));
                Console.WriteLine("Message sent!!!");
                Thread.Sleep(3000);
            }

            Console.ReadKey();
        }
    }
}
