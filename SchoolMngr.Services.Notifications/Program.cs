/// <summary>
/// 
/// </summary>
namespace SchoolMngr.Services.Notifications
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using SchoolMngr.Infrastructure.Shared.Configuration;
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

