using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Settings
{ 
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
