using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using IMongoDbSettings = GloboTicket.TicketManagement.Mongo.Persistence.Settings.IMongoDbSettings;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly ILogger _logger;
        private readonly ICategoryRepository _categoryRepository;

        public EventRepository(/*GloboTicketDbContext*/ /*IMongoDbContext */ IMongoDbSettings dbContext, ICategoryRepository categoryRepository, ILogger<Event> logger) : base(dbContext, logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            SeedData(_dbContext);
        }
        public static void SeedData(IMongoCollection<Event> dataCollection)
        {
            //check if there is any existing product collections
            bool existProduct = dataCollection.Find(p => true).Any();
            if (!existProduct)
            {
                dataCollection.InsertManyAsync(GetPreconfiguredEvent());

            }
        }
        private static IEnumerable<Event> GetPreconfiguredEvent()
        {
            var concertGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");

            return new List<Event>()
            {
                new Event()
                {
                Id = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                Name = "John Egbert Live",
                Price = 65,
                Artist = "John Egbert",
                Date = DateTime.Now.AddMonths(6),
                Description = "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",
                CategoryId = concertGuid
                }
            };
        }



        public async Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
        {
            _logger.LogInformation("GetCategoriesWithEvents Initiated");

            /* FilterDefinition<Event> filter = *//*Builders<Event>.Filter.AnyEq("Name", name) & *//*Builders<Event>.Filter.AnyEq("Date", eventDate.Date);
             var matches1 = await _dbContext.FindAsync(filter).Result.AnyAsync();
 */
            var matches = _dbContext.AsQueryable().Any(e => e.Name == name /*&& e.Date.Date==resDate*/);


            // await  _dbContext.FindAsync(filter).Result.FirstOrDefaultAsync();
            _logger.LogInformation("GetCategoriesWithEvents Completed");
            return await Task.FromResult(matches);
        }

        public async Task<Event> AddEventWithCategory(Event @event)
        {
            _logger.LogInformation("ListAllAsync Initiated");
            var categories = await _categoryRepository.ListAllAsync();
            var category = categories.FirstOrDefault(x => x.Name == @event.Category.Name);
            if (category != null)
            {
                @event.Category = category;
                @event.CategoryId = category.Id;
            }
            else
            {
                await _categoryRepository.AddAsync(@event.Category);
                @event.CategoryId = @event.Category./*Category*/Id;
            }

            await _dbContext.InsertOneAsync(@event);
            return @event;
        }







        /* public Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
         {
             _logger.LogInformation("GetCategoriesWithEvents Initiated");
             var matches =  _dbContext.Events.Any(e => e.Name.Equals(name) && e.Date.Date.Equals(eventDate.Date));
             _logger.LogInformation("GetCategoriesWithEvents Completed");
             return Task.FromResult(matches);
         }

         public async Task<Event> AddEventWithCategory(Event @event)
         {
           //  _dbContext.BeginTransaction();
             var categories = await _dbContext.Set<Category>().ToListAsync();
             var category = categories.FirstOrDefault(x => x.Name == @event.Category.Name);
             if (category != null)
             {
                 @event.Category = category;
                 @event.CategoryId = category.CategoryId;
             }
             else
             {
                 await _dbContext.Set<Category>().AddAsync(@event.Category);
                 await _dbContext.SaveChangesAsync();
                 @event.CategoryId = @event.Category.CategoryId;
             }
             await _dbContext.Set<Event>().AddAsync(@event);
             await _dbContext.SaveChangesAsync();
             _dbContext.Commit();
             return @event;
         }*/
    }
}
