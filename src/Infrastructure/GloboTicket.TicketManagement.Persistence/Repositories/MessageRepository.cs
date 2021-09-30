using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Persistence.Repositories
{
    public class MessageRepository : BaseRepository<Notification>, IMessageRepository
    {

        private readonly ILogger _logger;
        public MessageRepository(GloboTicketDbContext dbContext, ILogger<Notification> logger) : base(dbContext, logger)
        {
            _logger = logger;
        }

        public  Task<List<Notification>> GetAllNotifications()
        {
            _logger.LogInformation("GetAllNotifications Initiated");
            var Allnotifications =  _dbContext.NotificationMaster.Include(x => x.NotificationCode).ToListAsync();
            _logger.LogInformation("GetAllNotifications Completed");
            return Allnotifications;
        }
    }
}
