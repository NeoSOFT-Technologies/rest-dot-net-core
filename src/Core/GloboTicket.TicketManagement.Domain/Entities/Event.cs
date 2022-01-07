using GloboTicket.TicketManagement.Domain.Common;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GloboTicket.TicketManagement.Domain.Entities
{
    public class Event : AuditableEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Artist { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
