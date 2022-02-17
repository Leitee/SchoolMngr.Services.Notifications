using Codeit.Enterprise.Base.Abstractions.Desentralized;
using Codeit.Enterprise.Base.Desentralized.IntegrationEvent;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SchoolMngr.Infrastructure.Shared;
using SchoolMngr.Services.Notifications.Hubs;
using SchoolMngr.Services.Notifications.IntegrationEvents;
using Serilog;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Staging,
    WebRootPath = "customwwwroot"
});

builder.Logging.AddSerilog();
builder.Services
                .AddInfrastructureLayer("InfraSection")
                .AddIntegrationEventHandlers()
                .AddSignalR();

builder.Services
                .AddHealthChecks()
                .AddCheck("Self", _ => HealthCheckResult.Healthy());


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseSerilogRequestLogging()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/hc");
        endpoints.MapHub<NotificationsHub>("/hub/notificationhub", options => options.Transports = HttpTransports.All);
        endpoints.MapHub<StatusHub>("/hub/status");
    });

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

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