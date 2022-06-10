using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Persistence.Repositories;
using GloboTicket.TicketManagement.Persistence.UnitTests.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Persistence.UnitTests.Repositories
{
    public class EventRepositoryTests
    {
        private readonly Mock<GloboTicketDbContext> _mockDbContext;
        private readonly Mock<ILogger<Event>> _logger;

        public EventRepositoryTests()
        {
            _mockDbContext = GloboTicketDbContextMocks.GetGloboTicketDbContext();
            _logger = new Mock<ILogger<Event>>();
        }

        [Fact]
        public async Task Event_NameAndDate_Not_Unique()
        {
            var repo = new EventRepository(_mockDbContext.Object, _logger.Object);

            var result = await repo.IsEventNameAndDateUnique("The State of Affairs: Michael Live!", new DateTime(2027, 1, 18));

            result.ShouldBeOfType<bool>();
            result.ShouldBe(true);
        }

        [Fact]
        public async Task Event_NameAndDate_Unique()
        {
            var repo = new EventRepository(_mockDbContext.Object, _logger.Object);

            var result = await repo.IsEventNameAndDateUnique("The State of Affairs: Michael Live!", new DateTime(2027, 1, 19));

            result.ShouldBeOfType<bool>();
            result.ShouldBe(false);
        }

        //[Fact]
        public async Task AddEvent_WithExisting_Category()
        {
            var repo = new EventRepository(_mockDbContext.Object, _logger.Object);

            var result = await repo.AddEventWithCategory(new Event()
            {
                Name = "Test",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                Category = new Category()
                {
                    CategoryId = new Guid(),
                    Name = "Conferences"
                }
            });

            result.ShouldNotBeNull();
            result.ShouldBeOfType<Event>();
            var events = await _mockDbContext.Object.Set<Event>().ToListAsync();
            events.Count.ShouldBe(7);
            var categories = await _mockDbContext.Object.Set<Category>().ToListAsync();
            categories.Count.ShouldBe(4);
        }

        //[Fact]
        public async Task AddEvent_WithNew_Category()
        {
            var repo = new EventRepository(_mockDbContext.Object, _logger.Object);

            var result = await repo.AddEventWithCategory(new Event()
            {
                Name = "Test",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                Category = new Category()
                {
                    CategoryId = new Guid(),
                    Name = "CategoryTest"
                }
            });

            result.ShouldNotBeNull();
            result.ShouldBeOfType<Event>();
            var events = await _mockDbContext.Object.Set<Event>().ToListAsync();
            events.Count.ShouldBe(7);
            var categories = await _mockDbContext.Object.Set<Category>().ToListAsync();
            categories.Count.ShouldBe(5);
        }
    }
}
