using GloboTicket.TicketManagement.Api;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Base
{
    public class DbFixture : IDisposable
    {
        private readonly  GloboTicketDbContext _dbContext;
        public readonly string BlogDbName = $"Blog-{Guid.NewGuid()}";
        public readonly string ConnString;

        private bool _disposed;

        public DbFixture()
        {
            ConnString = $"Server=localhost,1433;Database={BlogDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";

            var builder = new DbContextOptionsBuilder<GloboTicketDbContext>();

            builder.UseSqlServer(ConnString);
            _dbContext = new GloboTicketDbContext(builder.Options);

            _dbContext.Database.Migrate();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // remove the temp db from the server once all tests are done
                    _dbContext.Database.EnsureDeleted();
                }

                _disposed = true;
            }
        }
    }

    [CollectionDefinition("Database")]
    public class DatabaseCollection : ICollectionFixture<DbFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }



    [Collection("Database")]
    public class BlogWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly DbFixture _dbFixture;

        public BlogWebApplicationFactory(DbFixture dbFixture)
            => _dbFixture = dbFixture;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            // UPDATE: No need to remove the original DbContext.
            // To use our Docker db, we can just provide an in-memory config provider.		
            // The original code is just here for reference.

            //builder.ConfigureServices(services =>
            //{
            //    // Remove the app's BlogDbContext registration.
            //    var descriptor = services.SingleOrDefault(
            //        d => d.ServiceType ==
            //            typeof(DbContextOptions<BlogDbContext>));

            //    if (descriptor is object)
            //        services.Remove(descriptor);

            //    services.AddDbContext<BlogDbContext>(options =>
            //    {
            //        // uses the connection string from the fixture
            //        options.UseSqlServer(_dbFixture.ConnString);
            //    });
            //})
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "ConnectionStrings:GloboTicketTicketManagementConnectionString", _dbFixture.ConnString)
                });
            });
        }
    }
}
