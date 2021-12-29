using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Mongo.Persistence.Repositories;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace GloboTicket.TicketManagement.Mongo.Persistence
{
    public static class PersistenceMongoServiceRegistration
    {
        public static IServiceCollection AddPersistenceMongoServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings")); /*ConnectionStrings1:*/

            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            //services.AddScoped<IMongoDbContext, MongoDbContext>();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            return services;
        }
    }
}
