using GloboTicket.TicketManagement.Domain.Common;
using System;

namespace GloboTicket.TicketManagement.Domain.Entities
{
    public class Notification : AuditableEntity
    {
        public Guid NotificationId { get; set; }
        public string NotificationCode { get; set; }
        public string NotificationMessage { get; set; }
    }
}
