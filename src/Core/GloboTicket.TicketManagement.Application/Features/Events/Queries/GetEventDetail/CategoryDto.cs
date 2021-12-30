using MongoDB.Bson;
using System;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class CategoryDto
    {
        public string/*ObjectId*//*Guid*/ /*Category*/Id { get; set; }
        public string Name { get; set; }
    }
}
