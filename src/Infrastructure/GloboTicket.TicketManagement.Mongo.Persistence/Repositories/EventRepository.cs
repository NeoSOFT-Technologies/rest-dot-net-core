using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly ILogger _logger;
        private readonly ICategoryRepository _categoryRepository;

        public EventRepository(IMongoDbSettings dbContext, ICategoryRepository categoryRepository, ILogger<Event> logger) : base(dbContext, logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;

        }


        public async Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
        {
            _logger.LogInformation("GetCategoriesWithEvents Initiated");

            var allEvents = await _dbContext.FindAsync(Builders<Event>.Filter.Empty).Result.ToListAsync();

            var matches = allEvents.AsQueryable().Any(e => e.Name == name && e.Date.Date == eventDate.Date);

            _logger.LogInformation("GetCategoriesWithEvents Completed");
            return await Task.FromResult(matches);
        }

        public async Task<Event> AddEventWithCategory(Event @event)
        {
            _logger.LogInformation("AddEventWithCategory Initiated");
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
                @event.CategoryId = @event.Category.Id;
            }

            await _dbContext.InsertOneAsync(@event);
            _logger.LogInformation("AddEventWithCategory completed");
            return @event;
        }


    }
}
