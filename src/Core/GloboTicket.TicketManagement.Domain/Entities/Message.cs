using System;
using GloboTicket.TicketManagement.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GloboTicket.TicketManagement.Domain.Entities
{
   /* [BsonCollection("Message")]*/
    public class Message : IDocument
    {

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public /*Guid*/ObjectId Id { get; set; }
        //public Guid MessageId { get; set; } /*=> Id;*/
        public string Code { get; set; }
        public string MessageContent { get; set; }
        public string Language { get; set; }
        public MessageType Type { get; set; }
        

        public enum MessageType
        {
            Information,
            Error,
            Warning
        }
    }
}
