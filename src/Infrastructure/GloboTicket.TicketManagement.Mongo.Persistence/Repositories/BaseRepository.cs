using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using IMongoDbSettings = GloboTicket.TicketManagement.Mongo.Persistence.Settings.IMongoDbSettings;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]

    public class BaseRepository<T> : IAsyncRepository<T> where T : class, IDocument
    {
        /* protected readonly IMongoCollection<T> _dbContext;
         private readonly ILogger _logger;
         //private readonly IMongoDatabase _database;


         public BaseRepository(IMongoDbSettings settings, ILogger<T> logger)
         {
             _logger = logger;
             var _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);

             _dbContext = _database.GetCollection<T>(GetCollectionName(typeof(T)));
             //  _dbContext.Database.GetCollection<T>("");
         }
         private protected string GetCollectionName(Type documentType)
         {
             return
                 ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                     typeof(BsonCollectionAttribute),
                     true)
                 .FirstOrDefault())?.CollectionName;
         }*/

        //protected readonly IMongoDbContext Context;
        protected IMongoCollection<T> _dbContext;//DbSet
        private readonly ILogger _logger;
        //private readonly IMongoDatabase _database;


        public BaseRepository(IMongoDbSettings settings/*IMongoDbContext settings*/, ILogger<T> logger)
        {
            _logger = logger;
           // Context = settings;

            var Context1= new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);

            _dbContext = Context1.GetCollection<T>(typeof(T).Name);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            _logger.LogInformation("ListAllAsync Initiated");
            var all = await _dbContext.FindAsync(Builders<T>.Filter.Empty);
            return await all.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            //var objectId = new ObjectId(id);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);

            return await _dbContext.FindAsync(filter).Result.FirstOrDefaultAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(T).Name + " object is null");
            }
            await _dbContext.InsertOneAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            /*  _dbContext.Entry(entity).State = EntityState.Modified;
              await _dbContext.SaveChangesAsync();*/
            await _dbContext.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", entity.Id), entity);

        }

        public async Task DeleteAsync(T entity)
        {
            await _dbContext.DeleteOneAsync(Builders<T>.Filter.Eq("_id", entity.Id));

        }


        public async virtual Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size)
        {
            return _dbContext.AsQueryable<T>().Skip((page - 1) * size).Take(size).ToList();
        }

    }
}
