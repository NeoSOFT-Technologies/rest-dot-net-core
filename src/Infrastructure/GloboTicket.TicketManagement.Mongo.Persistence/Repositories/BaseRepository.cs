using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
//using IMongoDbSettings = GloboTicket.TicketManagement.Mongo.Persistence.Settings.IMongoDbSettings;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]

    public class BaseRepository<T> : IAsyncRepository<T> where T : class, IDocument
    {
        

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

        public async Task<T> GetByIdAsync(/*ObjectId*/string id)
        {
             var objectId = new ObjectId(id);
        
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objectId);
            var result= await _dbContext.FindAsync(filter).Result.FirstOrDefaultAsync();
            return result;
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
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, entity.Id);
            await _dbContext.FindOneAndDeleteAsync(filter);
           //  _dbContext.DeleteOneAsync(Builders<T>.Filter.Eq("_id", entity.Id));

        }


        public async virtual Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size)
        {
            return _dbContext.AsQueryable<T>().Skip((page - 1) * size).Take(size).ToList();
        }




        //Extra
        public virtual IQueryable<T> AsQueryable()
        {
            return _dbContext.AsQueryable();
        }

        public virtual IEnumerable<T> FilterBy(
        Expression<Func<T, bool>> filterExpression)
        {
            return _dbContext.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TProjected>> projectionExpression)
        {
            return _dbContext.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual T FindOne(Expression<Func<T, bool>> filterExpression)
        {
            return _dbContext.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression)
        {
            return Task.Run(() => _dbContext.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual T FindById(string id)
        {
           // var objectId = new ObjectId/*Guid*/(id);
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            return _dbContext.Find(filter).SingleOrDefault();
        }

        


        public virtual void InsertOne(T document)
        {
            _dbContext.InsertOne(document);
        }

        public virtual Task InsertOneAsync(T document)
        {
            return Task.Run(() => _dbContext.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<T> documents)
        {
            _dbContext.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<T> documents)
        {
            await _dbContext.InsertManyAsync(documents);
        }

        public void ReplaceOne(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);
            _dbContext.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);
            await _dbContext.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<T, bool>> filterExpression)
        {
            _dbContext.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<T, bool>> filterExpression)
        {
            return Task.Run(() => _dbContext.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
          //  var objectId = new ObjectId/*Guid*/(id);
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            _dbContext.FindOneAndDelete(filter);
        }

        public  Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
              //  var objectId = new /*Guid*/ObjectId(id);
                var filter = Builders<T>.Filter.Eq(doc => doc.Id, id/*objectId*/);
                _dbContext.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<T, bool>> filterExpression)
        {
             _dbContext.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<T, bool>> filterExpression)
        {
            return Task.Run(() => _dbContext.DeleteManyAsync(filterExpression));
        }

    }
}
