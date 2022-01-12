using GloboTicket.TicketManagement.gRPC;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.gRPC.IntegrationTests.Base
{

    [Collection("Database")]
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly DbFixture _dbFixture;
        public GrpcChannel channel;
        public CustomWebApplicationFactory(DbFixture dbFixture)
        {
            _dbFixture = dbFixture;
            var client = CreateDefaultClient(new ResponseVersionHandler());
            channel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions()
                {
                HttpClient = client
                }
            ) ;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test"); 
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "ConnectionStrings:GloboTicketTicketManagementConnectionString", _dbFixture.ApplicationConnString),
                });
            });
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "ConnectionStrings:GloboTicketIdentityConnectionString", _dbFixture.IdentityConnString)
                });
            });
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "ConnectionStrings:GloboTicketHealthCheckConnectionString", _dbFixture.HealthCheckConnString)
                });
            });
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "CacheConfiguration:AbsoluteExpirationInHours", "1")
                });
            });
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "CacheConfiguration:SlidingExpirationInMinutes", "30")
                });
            });
        }
        private class ResponseVersionHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                var response = await base.SendAsync(request, cancellationToken);
                response.Version = request.Version;
                return response;
            }
        }
    }
}
