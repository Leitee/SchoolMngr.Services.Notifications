
Log.Logger = SharedHostConfiguration.CreateSerilogLogger(nameof(SchoolMngr.Services.Notifications));

Log.Information("Configuring web host ({ApplicationName})...");
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development
});

builder.Logging.AddSerilog(Log.Logger);

var services = builder.Services;
services.AddInfrastructureLayer(AppSettings.InfraSectionKey)
            .AddSignalR();

services.AddHealthChecks().AddCheck("Self", _ => HealthCheckResult.Healthy());

//Register event handlers
services.AddSingleton<ProccessGreetingIntegrationEventHandler>();
services.AddSingleton<ClientNotificationIntegrationEventHandler>();
        
//Register business services
services.AddSingleton<IClientNotificationService, ClientNotificationService>();

WebApplication app = builder.Build();

//Subscribe event by payloads
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<GreetingIntegrationEventPayload, ProccessGreetingIntegrationEventHandler>();
eventBus.Subscribe<ClientNotificationIntegrationEventPayload, ClientNotificationIntegrationEventHandler>();

app.UseRouting().UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/hc");
        endpoints.MapHub<NotificationsHub>("/hub/notification", options => options.Transports = HttpTransports.All);
        endpoints.MapHub<StatusHub>("/hub/status");
    });

try
{
    Log.Information("Starting web host ({ApplicationName})...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed at start up.");
}
finally
{
    Log.CloseAndFlush();
}
