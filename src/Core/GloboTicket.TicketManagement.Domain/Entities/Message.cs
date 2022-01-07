using System;
using GloboTicket.TicketManagement.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GloboTicket.TicketManagement.Domain.Entities
{
    public class Message : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
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
