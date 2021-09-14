using SchoolMngr.Services.Notifications.FunctionalTest.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading;
using Xunit;

namespace SchoolMngr.Services.Notifications.FunctionalTest.Scenarios
{
    public class HubScenariosTest : IClassFixture<HostFixture>
    {
        TestServer _server;

        public HubScenariosTest(HostFixture hostFixture)
        {
            _server = hostFixture.Host.GetTestServer();
        }

        [Fact(DisplayName = "Should pass message throught method")]
        public async void GetStatusReport()
        {
            //Arrange
            string greeting = String.Empty;
            string message = "helathy";
            var connection = new HubConnectionBuilder()
                .WithUrl(new Uri($"{_server.BaseAddress}hub/status"), op =>
                {
                    op.HttpMessageHandlerFactory = _ => _server.CreateHandler();
                })
                .WithAutomaticReconnect()
                .Build();

            connection.On<string>("OnBypassMsg", msg =>
            {
                greeting = msg;
            });

            //Act
            await connection.StartAsync();
            await connection.InvokeAsync("BypassMessage", message);

            Thread.Sleep(500);

            //Assert
            greeting.Should().Be(message);
        }
    }
}
