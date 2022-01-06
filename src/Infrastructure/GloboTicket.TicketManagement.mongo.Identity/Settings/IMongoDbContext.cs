using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.mongo.Identity.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.mongo.Identity.Settings
{ 
    public interface IMongoDbContext
    {
        //IMongoCollection<T> GetCollection<T>(string name);

        IMongoCollection<ApplicationUser> appUser { get; set; }
    }
}
