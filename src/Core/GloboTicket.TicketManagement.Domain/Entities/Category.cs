using GloboTicket.TicketManagement.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GloboTicket.TicketManagement.Domain.Entities
{/*
    [BsonCollection("Category")]*/
    public class Category : AuditableEntity
    {
        public Guid CategoryId /*{ get; set; }*/ => Id;
        public string Name { get; set; }
        public ICollection<Event> Events { get; set; }
       








        /*
        [Backlink(nameof(Event.CategoryId))]
        public IQueryable<Event> Eventss { get; }*/

    }

}
