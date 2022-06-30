using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Persistence.Repositories;
using GloboTicket.TicketManagement.Persistence.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Persistence.UnitTests.Repositories
{
    public class CategoryRepositoryTests
    {
        private readonly Mock<GloboTicketDbContext> _mockDbContext;
        private readonly Mock<ILogger<Category>> _logger;

        public CategoryRepositoryTests()
        {
            _mockDbContext = GloboTicketDbContextMocks.GetGloboTicketDbContext();
            _logger = new Mock<ILogger<Category>>();
        }

        [Fact]
        public async Task GetCategoriesWithEvents_IncludePassedEvents()
        {
            var repo = new CategoryRepository(_mockDbContext.Object, _logger.Object);

            var result = await repo.GetCategoriesWithEvents(true);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<Category>>();
            result.Count.ShouldBe(4);
            result.FirstOrDefault().Events.Count.ShouldBe(4);
        }

        [Fact]
        public async Task GetCategoriesWithEvents_DoNot_IncludePassedEvents()
        {
            var repo = new CategoryRepository(_mockDbContext.Object, _logger.Object);

            var result = await repo.GetCategoriesWithEvents(false);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<Category>>();
            result.Count.ShouldBe(4);
            result.FirstOrDefault().Events.Count.ShouldBe(4);
        }
    }
}
