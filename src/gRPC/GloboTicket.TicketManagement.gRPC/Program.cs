using GloboTicket.TicketManagement.Application;
using GloboTicket.TicketManagement.Application.Contracts;
using GloboTicket.TicketManagement.gRPC.LoggedServices;
using GloboTicket.TicketManagement.gRPC.Middleware;
using GloboTicket.TicketManagement.gRPC.Services;
using GloboTicket.TicketManagement.gRPC.Services.v1;
using GloboTicket.TicketManagement.Identity;
using GloboTicket.TicketManagement.Infrastructure;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = builder.Configuration;

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ListenAnyIP(10043);

    // BloomRPC needs pure HTTP/2 instead of HTTP/1.1+HTTP/2
    options.ListenLocalhost(5050, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDataProtection();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(Configuration);
builder.Services.AddPersistenceServices(Configuration);
builder.Services.AddIdentityServices(Configuration);
builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCustomExceptionHandler();
app.UseEndpoints(endpoints =>
{
    //For registeration of v1 services
    endpoints.MapGrpcService<EventService>();
    endpoints.MapGrpcService<CategoryService>();
    endpoints.MapGrpcService<AccountService>();

    //For registeration of v2 services
    endpoints.MapGrpcService<GloboTicket.TicketManagement.gRPC.Services.v2.CategoryService>();

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    });
});

app.Run();
