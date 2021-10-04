using System;

namespace GloboTicket.TicketManagement.Domain.Entities
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Language { get; set; }
    }
}
