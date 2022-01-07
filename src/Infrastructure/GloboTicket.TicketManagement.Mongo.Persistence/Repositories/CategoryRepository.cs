using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        protected readonly IMongoCollection<Category> _CatdbContext;
        private readonly ILogger _logger;
        public CategoryRepository(IMongoDbSettings dbContext, ILogger<Category> logger) : base(dbContext, logger)
        {
            _logger = logger;
            //SeedData(_dbContext);
        }

        public async Task<List<Category>> GetCategoriesWithEvents(bool includePassedEvents)
        {
            _logger.LogInformation("GetCategoriesWithEvents Initiated");

            FilterDefinition<Category> filter = Builders<Category>.Filter.Exists(x => x.Events);

            var allCategories = await _dbContext.FindAsync(filter).Result.ToListAsync();

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
