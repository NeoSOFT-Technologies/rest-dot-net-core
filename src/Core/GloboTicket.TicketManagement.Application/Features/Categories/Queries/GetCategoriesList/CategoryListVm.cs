using MongoDB.Bson;
using System;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList
{
    public class CategoryListVm
    {
        public string /*Guid*//*ObjectId*/ /*Category*/Id { get; set; }
        public string Name { get; set; }
    }
}
