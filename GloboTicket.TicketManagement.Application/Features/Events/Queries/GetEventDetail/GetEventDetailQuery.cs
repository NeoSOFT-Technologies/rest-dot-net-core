using GloboTicket.TicketManagement.Application.Responses;
using MediatR;
using System;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class GetEventDetailQuery: IRequest<Response<EventDetailVm>>
    {
        public Guid Id { get; set; }
    }
}
