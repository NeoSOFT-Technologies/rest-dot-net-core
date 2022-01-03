using GloboTicket.TicketManagement.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Base
{

    [Collection("Database")]
    public class WebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly DbFixture _dbFixture;

        public WebApplicationFactory(DbFixture dbFixture)
            => _dbFixture = dbFixture;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test"); 
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                  /*  new KeyValuePair<string, string>(
                        "ConnectionStrings:GloboTicketTicketManagementConnectionString", _dbFixture.ApplicationConnString),*/
                  new KeyValuePair<string,string>(
                      "MongoDbSettings:ConnectionString",_dbFixture.mongoDbSettings.ConnectionString),
                });

            });
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                  new KeyValuePair<string,string>(
                      "MongoDbSettings:DatabaseName",_dbFixture.ApplicationDbName),
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
    }
}
