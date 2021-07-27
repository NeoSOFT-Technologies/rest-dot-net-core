using GloboTicket.TicketManagement.App.ViewModels;
using System;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.App.Contracts
{
    public interface IOrderDataService
    {
        Task<PagedOrderForMonthViewModel> GetPagedOrderForMonth(DateTime date, int page, int size);
    }
}
