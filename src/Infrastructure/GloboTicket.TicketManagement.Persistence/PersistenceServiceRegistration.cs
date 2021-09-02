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
            var EncryptedString = configuration.GetConnectionString("GloboTicketTicketManagementConnectionString");
            var key = configuration.GetConnectionString("KeyValue");
            var encrypted = EncryptionDecryption.EncryptString(EncryptedString, key);
            var decrypted = EncryptionDecryption.DecryptString(encrypted, key);

            //Below lines of code is incomment when we put encrypted string into configuration files or at the time of deployemnt
            //Time being the code is commented because the connection string is locally configured.

            //services.AddDbContext<GloboTicketDbContext>(options =>
            //    options.UseSqlServer(decrypted.ToString()));
            services.AddDbContext<GloboTicketDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("GloboTicketTicketManagementConnectionString")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;    
        }
    }
}
