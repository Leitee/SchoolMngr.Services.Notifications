
namespace SchoolMngr.Services.Notifications
{
    using Codeit.NetStdLibrary.Base.Abstractions.Desentralized;
    using Codeit.NetStdLibrary.Base.Desentralized.IntegrationEvent;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using SchoolMngr.Infrastructure.Shared;
    using SchoolMngr.Services.Notifications.Hubs;
    using SchoolMngr.Services.Notifications.IntegrationEvents;
    using Serilog;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddInfrastructure("InfrastructureSection")
                .AddIntegrationEventHandlers()
                .AddSignalR();

            services.AddHealthChecks()
                .AddCheck("Self", _ => HealthCheckResult.Healthy());
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseSerilogRequestLogging()
                .UseEventBus()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/hc");
                    endpoints.MapHub<NotificationsHub>("/hub/notificationhub", options => options.Transports = HttpTransports.All);
                    endpoints.MapHub<StatusHub>("/hub/status");
                });
        }
    }

    static class StartUpExtensions
    {
        public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
        {
            //Regiter Handlers
            services.AddSingleton<ProccessGreetingIntegrationEventHandler>();
            services.AddSingleton<ClientNotificationIntegrationEventHandler>();
            return services;
        }

        public static IApplicationBuilder UseEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<GreetingIntegrationEventPayload, ProccessGreetingIntegrationEventHandler>();
            eventBus.Subscribe<ClientNotificationIntegrationEventPayload, ClientNotificationIntegrationEventHandler>();
            return app;
        }
    }
}
