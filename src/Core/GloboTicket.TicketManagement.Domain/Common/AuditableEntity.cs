using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GloboTicket.TicketManagement.Domain.Common
{
    public class AuditableEntity : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId /*Guid*/ Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
