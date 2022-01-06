using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
/*using Microsoft.Data.SqlClient;*/
/*using Microsoft.EntityFrameworkCore;*/
using Microsoft.Extensions.Logging;
using System;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
using MongoDB.Bson;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Repositories
{
    //[ExcludeFromCodeCoverage]
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        protected readonly IMongoCollection<Category> _CatdbContext;
        private readonly ILogger _logger;
        public CategoryRepository(/*GloboTicketDbContext*//*IMongoDbContext*/IMongoDbSettings dbContext, ILogger<Category> logger) : base(dbContext, logger)
        {
            //dbContext = (IMongoDbSettings)_dbContext.Database.GetCollection<Category>("Category");
            //_CatdbContext= Database.GetCollection<Category>("Category");
            _logger = logger;
            SeedData(_dbContext);
        }
        public static void SeedData(IMongoCollection<Category> dataCollection)
        {
            //check if there is any existing product collections
            bool existProduct = dataCollection.Find(p => true).Any();
            if (!existProduct)
            {
                dataCollection.InsertManyAsync(GetPreconfiguredCategory());

            }
        }
        private static IEnumerable<Category> GetPreconfiguredCategory()
        {
            var concertGuid = new string("61cc58c88b8879cc049839a8");
            var musicalGuid = new string("61d1cc89ace80d15f6249a53");
            //  /*Guid*/ ObjectId.Parse("61cc58c88b8879cc049839a8");
            // var concertGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            return new List<Category>()
            {
                new Category()
                {
                    Id = concertGuid,
                    Name = "Concerts"
                },
                 new Category()
                {
                 Id = musicalGuid,
                Name = "Musicals"
                }
            };
        }




        public async Task<List<Category>> GetCategoriesWithEvents(bool includePassedEvents)
        {
            _logger.LogInformation("GetCategoriesWithEvents Initiated");

            //  var allCategories = await _dbContext.AsQueryable().Include(x => x.Events).ToListAsync();
            //var allCategories = await _dbContext.AsQueryable().Select(x => x.Events).ToListAsync();
            /* var allCategories1 = _dbContext.AsQueryable().Select(ev => new
             {
                 cat= ev.Name,
                 Event = ev.Events.Select(e => e.Date)


             }).ToListAsync();
             */

            FilterDefinition<Category> filter = Builders<Category>.Filter.Exists(x => x.Events);

            var allCategories = await _dbContext.FindAsync(filter).Result.ToListAsync();

            // AsQueryable().Include(x => x.Events).ToListAsync()


            if (!includePassedEvents)
            {
                allCategories.ForEach(p => p.Events.ToList().RemoveAll(c => c.Date < DateTime.Today));
            }
            _logger.LogInformation("GetCategoriesWithEvents Completed");
            return allCategories;
        }

        public async Task<Category> AddCategory(Category category)
        {
            /* var categoryId = Guid.NewGuid();
             List<SqlParameter> parms = new List<SqlParameter>
                  {
                      // Create parameter(s)
                      new SqlParameter { ParameterName = "@CategoryId", Value = categoryId },
                      new SqlParameter { ParameterName = "@Name", Value = category.Name },
                  };
             await StoredProcedureCommandAsync("CreateCategory", parms.ToArray());
             category = await GetByIdAsync(categoryId);
             return category;*/
            throw new NotImplementedException();
        }

    }
}
