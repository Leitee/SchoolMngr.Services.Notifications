/// <summary>
/// 
/// </summary>
namespace Fitnner.Trainers.Notifications
{
    using Fitnner.Infrastructure.Shared;
    using Fitnner.Infrastructure.Shared.Configuration;
    using Fitnner.Trainers.Notifications.Hubs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Pandora.NetStdLibrary.Base.Abstractions.Desentralized;
    using Serilog;
    using System;

    public class Program
    {
        public static readonly string _appName = typeof(Program).Namespace;

        public static void Main(string[] args)
        {
            Log.Logger = SharedHostConfiguration.CreateSerilogLogger(_appName);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", _appName);
                var host = Host
                    .CreateDefaultBuilder(args)
                    .UseMetricsEndpoints()
                    .ConfigureWebHostDefaults(wb => wb.UseStartup<Startup>())
                    .UseSerilog()
                    .Build();

                Log.Information("Starting web host ({ApplicationContext})...", _appName);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed at start up");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}

