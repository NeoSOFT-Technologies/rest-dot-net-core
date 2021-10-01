using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Identity;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            ApplicationConnString = $"Server=localhost,1433;Database={ApplicationDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";
            IdentityConnString = $"Server=localhost,1433;Database={IdentityDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";
            HealthCheckConnString = $"Server=localhost,1433;Database={HealthCheckDbName};User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M";

            var applicationBuilder = new DbContextOptionsBuilder<GloboTicketDbContext>();
            var identityBuilder = new DbContextOptionsBuilder<GloboTicketIdentityDbContext>();

            applicationBuilder.UseSqlServer(ApplicationConnString);
            _applicationDbContext = new GloboTicketDbContext(applicationBuilder.Options);

            _applicationDbContext.Database.Migrate();

            var messages = new List<Notification>
            {
                new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationCode = "1",
                    NotificationMessage = "{PropertyName} is required.",
                    CreatedDate = DateTime.Now
                },
                new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationCode = "2",
                    NotificationMessage = "{PropertyName} must not exceed {MaxLength} characters.",
                    CreatedDate = DateTime.Now
                },
                new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationCode = "3",
                    NotificationMessage = "An event with the same name and date already exists.",
                    CreatedDate = DateTime.Now
                }
            };

            foreach(var item in messages)
            {
                _applicationDbContext.Add(item);
            }
            _applicationDbContext.SaveChanges();

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
