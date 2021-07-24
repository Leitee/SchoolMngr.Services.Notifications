/// <summary>
/// 
/// </summary>
namespace SchoolMngr.Services.Notifications
{
    using SchoolMngr.Infrastructure.Shared;
    using SchoolMngr.Services.Notifications.Hubs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Codeit.NetStdLibrary.Base.Abstractions.Desentralized;
    using Serilog;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddInfrastructureTier(Configuration)
                .AddIntegrationEventHandlers()
                .AddSignalR();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This will make the HTTP requests log as rich logs instead of plain text.
            app
                .UseSerilogRequestLogging()
                .UseEventBus()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
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
            services.AddTransient<ProccessGreetingIntegrationEventHandler>();
            return services;
        }

        public static IApplicationBuilder UseEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<GreetingIntegrationEventPayload, ProccessGreetingIntegrationEventHandler>();
            return app;
        }
    }
}
