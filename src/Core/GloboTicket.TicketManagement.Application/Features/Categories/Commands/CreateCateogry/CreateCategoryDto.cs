using MongoDB.Bson;
using System;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry
{
    public class CreateCategoryDto
    {
        public /*Guid*//*ObjectId*/ string /*Category*/Id { get; set; }
        public string Name { get; set; }
    }
}
