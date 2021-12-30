using MongoDB.Bson;
using System;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class EventDetailVm
    {
        public /*Guid*/ string /*ObjectId*//*Event*/Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Artist { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public /*Guid*//*ObjectId*/ string CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
