using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Persistence.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly string cacheKey = $"{typeof(Message)}";
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public MessageRepository(GloboTicketDbContext dbContext, ILogger<Message> logger, ICacheService cacheService) : base(dbContext, logger)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<string> GetMessage(string ErrorCode, string Lang)
        {
            _logger.LogInformation("GetAllNotifications Initiated");
            if (!_cacheService.TryGet(cacheKey, out IReadOnlyList<Message> cachedList))
            {
                cachedList = await _dbContext.Set<Message>().ToListAsync();
                _cacheService.Set(cacheKey, cachedList);
            }
            var error = cachedList.FirstOrDefault(x => x.ErrorCode == ErrorCode && x.Language == Lang);
            if(error == null)
            {
                cachedList = await _dbContext.Set<Message>().ToListAsync();
                _cacheService.Set(cacheKey, cachedList);
            }
            _logger.LogInformation("GetAllNotifications Completed");
            return error.ErrorMessage;
        }
    }
}
