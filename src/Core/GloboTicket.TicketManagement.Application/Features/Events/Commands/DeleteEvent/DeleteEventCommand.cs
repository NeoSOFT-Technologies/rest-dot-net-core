using MediatR;
using System;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommand: IRequest
    {
        public string /*Event*/Id { get; set; }
    }
}
