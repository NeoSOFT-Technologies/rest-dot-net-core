using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Persistence.Repositories;
using GloboTicket.TicketManagement.Persistence.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Persistence.UnitTests.Repositories
{
    public class MessageRepositoryTests
    {
        private readonly Mock<GloboTicketDbContext> _mockDbContext;
        private readonly Mock<ILogger<Message>> _logger;
        private readonly Mock<ICacheService> _cacheService;

        public MessageRepositoryTests()
        {
            _mockDbContext = GloboTicketDbContextMocks.GetGloboTicketDbContext();
            _logger = new Mock<ILogger<Message>>();
            _cacheService = new Mock<ICacheService>();
        }

        [Fact]
        public async Task GetMessageTest()
        {
            var repo = new MessageRepository(_mockDbContext.Object, _logger.Object, _cacheService.Object);

            var result = await repo.GetMessage("2", "en");

            result.ShouldNotBeNull();
            result.MessageContent.ShouldBe("{PropertyName} must not exceed {MaxLength} characters.");
        }
    }
}
