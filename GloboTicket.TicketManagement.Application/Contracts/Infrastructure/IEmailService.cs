using GloboTicket.TicketManagement.Application.Models.Mail;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
