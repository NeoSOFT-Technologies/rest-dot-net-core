using GloboTicket.TicketManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Contracts.Persistence
{
    public interface IMessageRepository : IAsyncRepository<Notification>
    {
        public Task<List<Notification>> GetAllNotifications();


    }
}
