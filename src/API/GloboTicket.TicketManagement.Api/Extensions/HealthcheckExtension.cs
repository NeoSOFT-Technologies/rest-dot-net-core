using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GloboTicket.TicketManagement.Api.Extensions
{
    public static class HealthcheckExtensionRegistration
    {
        public static IServiceCollection AddHealthcheckExtensionService(this IServiceCollection services, IConfiguration configuration)
        {
            var dbProvider = configuration.GetValue<string>("dbProvider");
            switch (dbProvider)
            {
                case "MSSQL":
                    services.AddHealthChecks()
                        .AddSqlServer(configuration["ConnectionStrings:GloboTicketIdentityConnectionString"], tags: new[] {
                            "db",
                            "all"})
                        .AddUrlGroup(new Uri(configuration["API:WeatertherInfo"]), tags: new[] {
                            "testdemoUrl",
                            "all"
                        });
                    services.AddHealthChecksUI(opt =>
                    {
                        opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
                        opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                        opt.SetApiMaxActiveRequests(1); //api requests concurrency
                        opt.AddHealthCheckEndpoint("API", "/healthz"); //map health check api
                    }).AddSqlServerStorage(configuration["ConnectionStrings:GloboTicketHealthCheckConnectionString"]);
                    break;
                case "PGSQL":
                    services.AddHealthChecks()
                        .AddNpgSql(configuration["ConnectionStrings:GloboTicketIdentityConnectionString"], tags: new[] {
                            "db",
                            "all"})
                        .AddUrlGroup(new Uri(configuration["API:WeatertherInfo"]), tags: new[] {
                            "testdemoUrl",
                            "all"
                        });
                    services.AddHealthChecksUI(opt =>
                    {
                        opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
                        opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                        opt.SetApiMaxActiveRequests(1); //api requests concurrency
                        opt.AddHealthCheckEndpoint("API", "/healthz"); //map health check api
                    }).AddPostgreSqlStorage(configuration["ConnectionStrings:GloboTicketHealthCheckConnectionString"]);
                    break;
                case "MySQL":
                    services.AddHealthChecks()
                        .AddMySql(configuration["ConnectionStrings:GloboTicketIdentityConnectionString"], tags: new[] {
                            "db",
                            "all"})
                        .AddUrlGroup(new Uri(configuration["API:WeatertherInfo"]), tags: new[] {
                            "testdemoUrl",
                            "all"
                        });
                    services.AddHealthChecksUI(opt =>
                    {
                        opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
                        opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                        opt.SetApiMaxActiveRequests(1); //api requests concurrency
                        opt.AddHealthCheckEndpoint("API", "/healthz"); //map health check api
                    }).AddMySqlStorage(configuration["ConnectionStrings:GloboTicketHealthCheckConnectionString"]);
                    break;
                default:
                    break;
            }

            return services;
        }
    }
}
