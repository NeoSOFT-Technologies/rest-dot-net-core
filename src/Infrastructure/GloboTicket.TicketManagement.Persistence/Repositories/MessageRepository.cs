using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Persistence.Repositories
{
    public class MessageRepository : BaseRepository<Notification>, IMessageRepository
    {
        private readonly string cacheKey = $"{typeof(Notification)}";
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public MessageRepository(GloboTicketDbContext dbContext, ILogger<Notification> logger, ICacheService cacheService) : base(dbContext, logger)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IReadOnlyList<Notification>> GetAllNotifications()
        {
            _logger.LogInformation("GetAllNotifications Initiated");
            if (!_cacheService.TryGet(cacheKey, out IReadOnlyList<Notification> cachedList))
            {
                cachedList = await _dbContext.Set<Notification>().ToListAsync();
                _cacheService.Set(cacheKey, cachedList);
            }
            _logger.LogInformation("GetAllNotifications Completed");
            return cachedList;
        }
    }
}
