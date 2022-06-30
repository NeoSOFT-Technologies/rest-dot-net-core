using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Persistence.Repositories;
using GloboTicket.TicketManagement.Persistence.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Persistence.UnitTests.Repositories
{
    public class OrderDepositoryTests
    {
        private readonly Mock<GloboTicketDbContext> _mockDbContext;
        private readonly Mock<ILogger<Order>> _logger;

        public OrderDepositoryTests()
        {
            _mockDbContext = GloboTicketDbContextMocks.GetGloboTicketDbContext();
            _logger = new Mock<ILogger<Order>>();
        }

        [Fact]
        public async Task GetPagedOrdersForMonthTest()
        {
            var repo = new OrderRepository(_mockDbContext.Object, _logger.Object);

            var result = await repo.GetPagedOrdersForMonth(new DateTime(2021, 8, 26), 3, 2);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<Order>>();
            result.Count.ShouldBe(2);
            result.FirstOrDefault().Id.ShouldBe(Guid.Parse("{E6A2679C-79A3-4EF1-A478-6F4C91B405B6}"));
        }

        [Fact]
        public async Task GetTotalCountOfOrdersForMonthTest()
        {
            var repo = new OrderRepository(_mockDbContext.Object, _logger.Object);

            var result = await repo.GetTotalCountOfOrdersForMonth(new DateTime(2021, 8, 26));

            result.ShouldBeOfType<int>();
            result.ShouldBe(7);
        }
    }
}
