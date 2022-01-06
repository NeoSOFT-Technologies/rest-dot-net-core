using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.mongo.Identity.Settings
{
    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
       
        
    }
}
