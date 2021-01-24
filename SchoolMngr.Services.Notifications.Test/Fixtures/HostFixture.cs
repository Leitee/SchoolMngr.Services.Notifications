using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pandora.NetStdLibrary.Base.Abstractions.Tests;
using Pandora.NetStdLibrary.Base.DataAccess;
using Fitnner.Trainers.Notifications.Hubs;

namespace Fitnner.Trainers.Notifications.FunctionalTest.Fixtures
{
    public class HostFixture : IHostFixture
    {
        private TestServer _server;
        public IWebHost Host => _server.Host;

        public HostFixture()
        {
            IWebHostBuilder builder = 
                new WebHostBuilder()
                    .ConfigureServices(services =>
                    {
                        services.AddSignalR();
                    })
                    .Configure(app =>
                    {
                        app.UseResponseCompression();
                        app.UseEndpoints(routes => routes.MapHub<StatusHub>("/hub/status"));
                    });

            _server = new TestServer(builder);
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }

    class StartupTest
    {
        private readonly IConfiguration _configuration;

        public StartupTest(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration[$"{nameof(DALSettings)}:UseDatabase"] = "false";
            _configuration[$"{nameof(DALSettings)}:DatabaseName"] = "IntegrationTestDB";
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ILoggerFactory>(sp => NullLoggerFactory.Instance)
                .AddTransient(sp => _configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}
