using System.Net.Http;

namespace GloboTicket.TicketManagement.App.Services
{
    public partial interface IClient
    {
        public HttpClient HttpClient { get; }

    }
}
