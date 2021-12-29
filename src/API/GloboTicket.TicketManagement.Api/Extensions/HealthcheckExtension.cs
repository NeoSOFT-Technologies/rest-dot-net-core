

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace GloboTicket.TicketManagement.Api.Extensions
{
  

    public static class HealthcheckExtensionRegistration
    {
        public static IServiceCollection AddHealthcheckExtensionService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
              .AddMongoDb(mongodbConnectionString: configuration.GetValue<string>("MongoDbSettings:ConnectionString"), mongoDatabaseName: configuration.GetValue<string>("MongoDbSettings:DatabaseName"), name: "mongo", failureStatus: HealthStatus.Unhealthy, tags: new[] {
              "db",
              "all"}) //adding MongoDb Health Check
            .AddSqlServer(configuration["ConnectionStrings:GloboTicketIdentityConnectionString"], tags: new[] {
                "db",
                "all"})
            .AddUrlGroup(new Uri(configuration["API:WeatertherInfo"]), tags: new[] {
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
