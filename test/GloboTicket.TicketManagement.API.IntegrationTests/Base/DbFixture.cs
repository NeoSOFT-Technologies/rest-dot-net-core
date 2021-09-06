using GloboTicket.TicketManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Base
{
    /// <summary>
    /// A collection fixture that is responsible for creating and dropping the database
    /// https://xunit.net/docs/shared-context
    /// </summary>
    public class DbFixture : IDisposable
    {
        private readonly GloboTicketDbContext _dbContext;
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
}
