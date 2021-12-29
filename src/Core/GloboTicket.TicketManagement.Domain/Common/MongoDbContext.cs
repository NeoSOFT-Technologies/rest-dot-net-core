using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Domain.Common
{
	public class MongoDbContext : IMongoDbContext
	{
		
		private IMongoDatabase Database { get; set; }
		public IClientSessionHandle Session { get; set; }
		public MongoClient MongoClient { get; set; }
		private readonly IConfiguration _configuration;

		//protected readonly MongoDbSettings _setting;

		public MongoDbContext( IConfiguration configuration)
		{
			_configuration = configuration;
			
		}

		public IMongoCollection<T> GetCollection<T>(string name)
		{
			//throw new NotImplementedException();
			ConfigureMongo();

			return Database.GetCollection<T>(name);
		}


		private void ConfigureMongo()
		{
			if (MongoClient != null)
			{
				return;
			}

            // Configure mongo (You can inject the config, just to simplify)
            MongoClient = new MongoClient(_configuration.GetValue<string>("MongoDbSettings:ConnectionString"));
            /*(_configuration["MongoDbSettings:ConnectionString"])*/


                Database = MongoClient.GetDatabase(_configuration.GetValue<string>("MongoDbSettings:DatabaseName"));
            /*(_configuration["MongoDbSettings:DatabaseName"])*/

        /*	MongoClient = new MongoClient(settings.ConnectionString)*//*(_configuration["MongoDbSettings:ConnectionString"])*//*;

			Database = MongoClient.GetDatabase(settings.DatabaseName);*/
        }
	}
}
