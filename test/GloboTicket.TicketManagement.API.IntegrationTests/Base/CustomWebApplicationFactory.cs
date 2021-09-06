using GloboTicket.TicketManagement.Api;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Base
{
    public class CustomWebApplicationFactory
            : WebApplicationFactory<Startup>
    {

        private readonly DbFixture _dbFixture;

        public CustomWebApplicationFactory()
        { 
            
            _dbFixture = new DbFixture(); 
        
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "ConnectionStrings:BlogConnection", _dbFixture.ConnString)
                });
            });

            //builder.ConfigureServices(services =>
            //{

            //    services.AddDbContext<GloboTicketDbContext>(options =>
            //    {
            //        options.UseInMemoryDatabase("GloboTicketDbContextInMemoryTest");
            //    });

            //    var sp = services.BuildServiceProvider();

            //    using (var scope = sp.CreateScope())
            //    {
            //        var scopedServices = scope.ServiceProvider;
            //        var context = scopedServices.GetRequiredService<GloboTicketDbContext>();
            //        var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            //        context.Database.EnsureCreated();

            //        try
            //        {
            //            Utilities.InitializeDbForTests(context);
            //        }
            //        catch (Exception ex)
            //        {
            //            logger.LogError(ex, $"An error occurred seeding the database with test messages. Error: {ex.Message}");
            //        }
            //    };
            //});
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }
    }
}
