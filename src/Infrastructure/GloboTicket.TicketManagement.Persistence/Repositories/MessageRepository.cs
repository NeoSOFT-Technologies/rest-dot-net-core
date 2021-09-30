using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Models.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Persistence.Repositories
{
    public class MessageRepository : BaseRepository<Notificationss>, IMessageRepository
    {

        private readonly ILogger _logger;
        public MessageRepository(GloboTicketDbContext dbContext, ILogger<Notificationss> logger) : base(dbContext, logger)
        {
            _logger = logger;
        }

        public  Task<List<Notificationss>> GetAllNotifications()
        {

           var Allnotifications =  _dbContext.NotificationMaster.Include(x => x.NotificationCode).ToListAsync();
            return Allnotifications;

          
        }

      
        

    }
}
