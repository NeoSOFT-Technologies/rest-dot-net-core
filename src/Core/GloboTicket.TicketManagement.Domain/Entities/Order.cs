using GloboTicket.TicketManagement.Domain.Common;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GloboTicket.TicketManagement.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public string UserId { get; set; }
        public int OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }
        public bool OrderPaid { get; set; }
    }
}
