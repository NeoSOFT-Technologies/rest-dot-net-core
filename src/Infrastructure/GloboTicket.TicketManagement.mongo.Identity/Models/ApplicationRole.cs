using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.mongo.Identity.Models
{
    [CollectionName("Roles")]
    public class ApplicationRole : MongoIdentityRole<string>
    {
       
    }
}
