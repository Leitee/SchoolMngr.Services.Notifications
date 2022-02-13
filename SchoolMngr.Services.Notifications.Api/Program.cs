
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development
});

builder.Logging.AddSerilog();
builder.Services
                .AddInfrastructureLayer("InfraSection")
                .AddIntegrationEventHandlers()
                .AddSignalR();

builder.Services
                .AddHealthChecks()
                .AddCheck("Self", _ => HealthCheckResult.Healthy());

builder.Services
                .AddScoped<IClientNotificationService, ClientNotificationService>();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/hc");
        endpoints.MapHub<NotificationsHub>("/hub/notificationhub", options => options.Transports = HttpTransports.All);
        endpoints.MapHub<StatusHub>("/hub/status");
    });


app.MapGet("/echo", () =>
{
    return $"{typeof(Program).Assembly.FullName} service is running";
})
.WithName("GetApplicationStatus");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
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