using GloboTicket.TicketManagement.Domain.Common;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GloboTicket.TicketManagement.Domain.Entities
{
    /*[BsonCollection("Order")]*/
    public class Order : AuditableEntity
    {

     //   public Guid Id /*{ get; set; }*/ => Id;
        public /*Guid*/ObjectId UserId { get; set; }
        public int OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }
        public bool OrderPaid { get; set; }
    }
}
