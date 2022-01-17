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
        protected readonly IMongoCollection<Event> EventCollection;
        private readonly ILogger _logger;

        public CategoryRepository(IMongoDbSettings dbContext, ILogger<Category> logger) : base(dbContext, logger)
        {
            _logger = logger;
            EventCollection = _dbContext.Database.GetCollection<Event>("Event");
        }

        public async Task<List<Category>> GetCategoriesWithEvents(bool includePassedEvents)
        {
            _logger.LogInformation("GetCategoriesWithEvents Initiated");
            var allCategories = await _dbContext.FindAsync(Builders<Category>.Filter.Empty).Result.ToListAsync();
            var CategoriesWithEvents = (from c in allCategories.AsQueryable()
                                        join e in EventCollection.AsQueryable()
                                        on c.Id equals e.CategoryId into eventList
                                        from e in eventList.DefaultIfEmpty()
                                        select new Category()
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            Events = e == null ? Array.Empty<Event>() : (ICollection<Event>)eventList,
                                        }).ToList();

            if (!includePassedEvents)
            {
                // CategoriesWithEvents.ForEach(p => p.Events.ToList().RemoveAll(c => c.Date < DateTime.Today));
                CategoriesWithEvents = (from c in allCategories.AsQueryable()
                                        join e in EventCollection.AsQueryable()
                                        on c.Id equals e.CategoryId into eventList
                                        from e in eventList.Where(e => e.Date > DateTime.Today).DefaultIfEmpty()
                                        select new Category()
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            Events = e == null ? Array.Empty<Event>() : (ICollection<Event>)eventList,
                                        }).ToList();
            }
            _logger.LogInformation("GetCategoriesWithEvents Completed");
            return CategoriesWithEvents;
        }

        public async Task<Category> AddCategory(Category category)
        {
            throw new NotImplementedException();

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
        }

    }
}
