
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Domain.Common
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string CollectionName { get; set; }
    }
}
