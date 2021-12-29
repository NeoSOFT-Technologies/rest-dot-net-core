
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Settings
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
