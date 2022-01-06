using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.mongo.Identity.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.mongo.Identity.Settings
{
    public class MongoDbContext : IMongoDbContext
    {
       // private IMongoDatabase Database { get; set; }
        public MongoClient MongoClient { get; set; }
        public MongoDbContext(IConfiguration configuration)
		{
            MongoClient = new MongoClient(configuration.GetValue<string>("MongoDbSettings:ConnectionString"));



          var  Database = MongoClient.GetDatabase(configuration.GetValue<string>("MongoDbSettings:DatabaseName"));

            appUser = Database.GetCollection<ApplicationUser>("ApplicationUser");

        }

        public IMongoCollection<ApplicationUser> appUser { get; set; }
    }
}

