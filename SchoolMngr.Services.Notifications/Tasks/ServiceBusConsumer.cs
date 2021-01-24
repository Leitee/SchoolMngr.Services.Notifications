using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pandora.NetStdLibrary.Base.Abstractions.Desentralized;
using Pandora.NetStdLibrary.Base.Common;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fitnner.Trainers.Notifications
{
    public class ServiceBusConsumer : BackgroundService
    {
        // Connection String for the namespace can be obtained from the Azure portal under the 
        // 'Shared Access policies' section.
        private readonly ILogger<ServiceBusConsumer> _logger;
        private readonly BaseSettings _settings;
        private readonly IEventBus _eventBus;

        public ServiceBusConsumer(IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory)
        {
            _settings = BaseSettings.GetSection(configuration ?? throw new ArgumentNullException(nameof(configuration)));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<ServiceBusConsumer>();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"{nameof(ServiceBusConsumer)} starts handling execution.");
            RegisterOnMessageHandlerAndReceiveMessages();
            return Task.CompletedTask;
        }

        void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that will process messages
            //queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            // Complete the message so that it is not received again.
            // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
            //await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
            // to avoid unnecessary exceptions.
        }

        Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
