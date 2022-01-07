using GloboTicket.TicketManagement.Application.Responses;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class GetEventDetailQuery : IRequest<Response<EventDetailVm>>
    {
        public string Id { get; set; }
    }
}
