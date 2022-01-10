using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly ILogger _logger;
        private readonly ICategoryRepository _categoryRepository;

        public EventRepository(GloboTicketDbContext dbContext, ICategoryRepository categoryRepository, ILogger<Event> logger) : base(dbContext, logger)
        {
            _logger = logger;
        }



        public Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
        {
            _logger.LogInformation("GetCategoriesWithEvents Initiated");
            var matches = _dbContext.Events.Any(e => e.Name.Equals(name) && e.Date.Date.Equals(eventDate.Date));
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
                @event.CategoryId = category.Id;
            }
            else
            {
                await _dbContext.Set<Category>().AddAsync(@event.Category);
                await _dbContext.SaveChangesAsync();
                @event.CategoryId = @event.Category.Id;
            }
            await _dbContext.Set<Event>().AddAsync(@event);
            await _dbContext.SaveChangesAsync();
            _dbContext.Commit();
            return @event;
        }
    }
}
