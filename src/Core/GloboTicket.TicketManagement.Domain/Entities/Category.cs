using GloboTicket.TicketManagement.Domain.Common;
using System;
using System.Collections.Generic;

namespace GloboTicket.TicketManagement.Domain.Entities
{
    public class Category: AuditableEntity
    {

       // public Guid CategoryId /*{ get; set; }*/=> Id;
        public string Name { get; set; }
        public ICollection<Event> Events { get; set; } 
        
    }
}
