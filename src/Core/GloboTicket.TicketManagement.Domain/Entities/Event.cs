using GloboTicket.TicketManagement.Domain.Common;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GloboTicket.TicketManagement.Domain.Entities
{
   /* [BsonCollection("Event")]*/
    public class Event : AuditableEntity
    {
        public Guid EventId /*{ get; set; }*/ => Id;
        public string Name { get; set; }
        public int Price { get; set; }
        public string Artist { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
