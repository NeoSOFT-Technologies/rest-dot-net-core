using GloboTicket.TicketManagement.Identity;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using Xunit;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Base
{
    public class DbFixture : IDisposable
    {
        //  private readonly  GloboTicketDbContext _applicationDbContext;
        private readonly GloboTicketIdentityDbContext _identityDbContext;
        public readonly string ApplicationDbName = $"Application-{Guid.NewGuid()}";
        public readonly string IdentityDbName = $"Identity-{Guid.NewGuid()}";
        public readonly string HealthCheckDbName = $"HealthCheck";
        public readonly string HealthCheckConnString;
        public readonly string ApplicationConnString;
        public readonly string IdentityConnString;
        public MongoDbSettings mongoDbSettings { get; }
        private bool _disposed;

        public DbFixture()
        {
            /* ApplicationConnString = $"Server=localhost,1433;Database={ApplicationDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";*/
            IdentityConnString = $"Server=localhost,1433;Database={IdentityDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";
            HealthCheckConnString = $"Server=localhost,1433;Database={HealthCheckDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";

            //  var applicationBuilder = new DbContextOptionsBuilder<GloboTicketDbContext>();

            var identityBuilder = new DbContextOptionsBuilder<GloboTicketIdentityDbContext>();
            /*var config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .Build();*/

            var connString = "mongodb://localhost:27018";
            var dbName = ApplicationDbName;
            /* applicationBuilder.UseSqlServer(ApplicationConnString);
             _applicationDbContext = new GloboTicketDbContext(applicationBuilder.Options);

             _applicationDbContext.Database.Migrate();
 */
            this.mongoDbSettings = new MongoDbSettings()
            {
                ConnectionString = connString,
                DatabaseName = dbName
            };

            identityBuilder.UseSqlServer(IdentityConnString);
            _identityDbContext = new GloboTicketIdentityDbContext(identityBuilder.Options);

            _identityDbContext.Database.Migrate();

            SeedIdentity seed = new SeedIdentity(_identityDbContext);
            seed.SeedUsers();
            seed.SeedUserRoles();
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
                    var client = new MongoClient(this.mongoDbSettings.ConnectionString);
                    client.DropDatabase(this.mongoDbSettings.DatabaseName);

                    //    _applicationDbContext.Database.EnsureDeleted();
                    _identityDbContext.Database.EnsureDeleted();
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
}
