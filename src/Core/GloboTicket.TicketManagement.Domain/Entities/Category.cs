using GloboTicket.TicketManagement.Domain.Common;
using System;
using System.Collections.Generic;

namespace GloboTicket.TicketManagement.Domain.Entities
{
    public class Category : AuditableEntity
    {

        public string Name { get; set; }
        public ICollection<Event> Events { get; set; }

    }
}
