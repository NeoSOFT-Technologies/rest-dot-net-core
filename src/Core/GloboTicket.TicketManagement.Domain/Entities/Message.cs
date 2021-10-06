using System;

namespace GloboTicket.TicketManagement.Domain.Entities
{
    public class Message
    {
        public Guid MessageId { get; set; }
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
