using System;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport
{
    public class EventExportDto
    {
        public string/*Guid*/ /*Event*/Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
