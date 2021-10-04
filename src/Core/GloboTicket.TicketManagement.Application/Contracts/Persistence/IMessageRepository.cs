using GloboTicket.TicketManagement.Domain.Entities;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Contracts.Persistence
{
    public interface IMessageRepository : IAsyncRepository<Message>
    {
        public Task<Message> GetMessage(string Code, string Lang);
    }
}
