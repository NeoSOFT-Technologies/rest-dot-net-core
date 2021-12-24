using GloboTicket.TicketManagement.Identity;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Base
{
    public class DbFixture : IDisposable
    {
        private readonly  GloboTicketDbContext _applicationDbContext;
        private readonly GloboTicketIdentityDbContext _identityDbContext;
        public readonly string ApplicationDbName = $"Application-{Guid.NewGuid()}";
        public readonly string IdentityDbName = $"Identity-{Guid.NewGuid()}";
        public readonly string HealthCheckDbName = $"HealthCheck";
        public readonly string HealthCheckConnString;
        public readonly string ApplicationConnString;
        public readonly string IdentityConnString;

        private bool _disposed;

        public DbFixture()
        {
            var applicationBuilder = new DbContextOptionsBuilder<GloboTicketDbContext>();
            var identityBuilder = new DbContextOptionsBuilder<GloboTicketIdentityDbContext>();

            var dbProvider = Environment.GetEnvironmentVariable("dbProvider") != null
                                ? Environment.GetEnvironmentVariable("dbProvider") : "MSSQL";
            switch (dbProvider)
            {
                case "MSSQL":
                    ApplicationConnString = $"Server=localhost,1433;Database={ApplicationDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";
                    IdentityConnString = $"Server=localhost,1433;Database={IdentityDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";
                    HealthCheckConnString = $"Server=localhost,1433;Database={HealthCheckDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";
                    applicationBuilder.UseSqlServer(ApplicationConnString);
                    identityBuilder.UseSqlServer(IdentityConnString);
                    break;
                case "PGSQL":
                    ApplicationConnString = $"Server=localhost;Port=5432;Database={ApplicationDbName};User Id=root;Password=root;";
                    IdentityConnString = $"Server=localhost;Port=5432;Database={IdentityDbName};User Id=root;Password=root;";
                    HealthCheckConnString = $"Server=localhost;Port=5432;Database={HealthCheckDbName};User Id=root;Password=root;";
                    applicationBuilder.UseNpgsql(ApplicationConnString);
                    identityBuilder.UseNpgsql(IdentityConnString);
                    break;
                case "MySQL":
                    ApplicationConnString = $"Server=localhost;Port=3306;Database={ApplicationDbName};Userid=root;Password=root;";
                    IdentityConnString = $"Server=localhost;Port=3306;Database={IdentityDbName};Userid=root;Password=root;";
                    HealthCheckConnString = $"Server=localhost;Port=3306;Database={HealthCheckDbName};Userid=root;Password=root;";
                    applicationBuilder.UseMySql(ApplicationConnString);
                    identityBuilder.UseMySql(IdentityConnString);
                    break;
                default:
                    break;
            }

            _applicationDbContext = new GloboTicketDbContext(applicationBuilder.Options);
            _applicationDbContext.Database.EnsureCreated();

            _identityDbContext = new GloboTicketIdentityDbContext(identityBuilder.Options);
            _identityDbContext.Database.EnsureCreated();

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
                    _applicationDbContext.Database.EnsureDeleted();
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
