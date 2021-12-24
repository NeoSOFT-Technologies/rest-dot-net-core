using GloboTicket.TicketManagement.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Xunit;

namespace GloboTicket.TicketManagement.API.UnitTests.Controllers
{
    public class StartupTests
    {
        [Fact]
        public void StartupTest()
        {
            var webHost = Should.NotThrow(() => Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<MyStartup>().Build());
            webHost.ShouldNotBeNull();
        }
    }

    public class MyStartup : Startup
    {
        public MyStartup(IConfiguration config) : base(config) { }
    }
}
