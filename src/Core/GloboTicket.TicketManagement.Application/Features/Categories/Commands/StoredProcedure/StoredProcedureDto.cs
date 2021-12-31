using System;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.StoredProcedure
{
    public class StoredProcedureDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
    }
}
