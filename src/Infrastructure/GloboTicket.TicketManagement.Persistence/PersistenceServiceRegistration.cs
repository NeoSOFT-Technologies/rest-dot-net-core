using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Infrastructure.EncryptDecrypt;
using GloboTicket.TicketManagement.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GloboTicket.TicketManagement.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var dbProvider = configuration.GetValue<string>("dbProvider");

            switch (dbProvider)
            {
                case "MSSQL":
                    services.AddDbContext<GloboTicketDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("GloboTicketTicketManagementConnectionString")));
                    break;
                case "PGSQL":
                    services.AddDbContext<GloboTicketDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("GloboTicketTicketManagementConnectionString")));
                    break;
                case "MySQL":
                    services.AddDbContext<GloboTicketDbContext>(options =>
                        options.UseMySql(configuration.GetConnectionString("GloboTicketTicketManagementConnectionString")));
                    break;
                default:
                    break;
            }

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            return services;    
        }
    }
}
