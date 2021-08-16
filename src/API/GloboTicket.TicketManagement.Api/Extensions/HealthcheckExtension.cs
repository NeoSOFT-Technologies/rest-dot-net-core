

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GloboTicket.TicketManagement.Api.Extensions
{
  

    public static class HealthcheckExtensionRegistration
    {
        public static IServiceCollection AddHealthcheckExtensionService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
            .AddSqlServer(configuration["ConnectionStrings:GloboTicketIdentityConnectionString"], tags: new[] {
                "db",
                "all"})
            .AddUrlGroup(new Uri("https://localhost:44318/weatherforecast"), tags: new[] {
                "testdemoUrl",
                "all"
            });

            //adding healthchecks UI
            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                opt.SetApiMaxActiveRequests(1); //api requests concurrency
                opt.AddHealthCheckEndpoint("API", "/healthz"); //map health check api
            }).AddSqlServerStorage(configuration["ConnectionStrings:GloboTicketHealthCheckConnectionString"]);
            return services;
        }
    }


}
