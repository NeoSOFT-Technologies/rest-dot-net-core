using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Persistence.Repositories;
using GloboTicket.TicketManagement.Persistence.UnitTests.Mocks;
using Microsoft.EntityFrameworkCore;
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
    public class BaseRepositoryTests
    {
        private readonly Mock<GloboTicketDbContext> _mockDbContext;
        private readonly Mock<ILogger<Event>> _logger;

        public BaseRepositoryTests()
        {
            _mockDbContext = GloboTicketDbContextMocks.GetGloboTicketDbContext();
            _logger = new Mock<ILogger<Event>>();
        }

        [Fact]
        public async Task ListAllAsyncTest()
        {
            var repo = new BaseRepository<Event>(_mockDbContext.Object, _logger.Object);

            var result = await repo.ListAllAsync();

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<Event>>();
            result.Count.ShouldBe(6);
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            var repo = new BaseRepository<Event>(_mockDbContext.Object, _logger.Object);

            var result = await repo.GetByIdAsync(Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"));

            result.ShouldNotBeNull();
            result.ShouldBeOfType<Event>();
            result.EventId.ShouldBe(Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"));
        }

        [Fact]
        public async Task GetPagedResponseAsyncTest()
        {
            var repo = new BaseRepository<Event>(_mockDbContext.Object, _logger.Object);

            var result = await repo.GetPagedReponseAsync(3, 2);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<Event>>();
            result.Count.ShouldBe(2);
            result.FirstOrDefault().EventId.ShouldBe(Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"));
        }

        [Fact]
        public async Task AddAsyncTest()
        {
            var repo = new BaseRepository<Event>(_mockDbContext.Object, _logger.Object);

            var result = await repo.AddAsync(new Event()
            {
                Name = "Test",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            });

            var events = await _mockDbContext.Object.Set<Event>().ToListAsync();
            events.Count.ShouldBe(7);
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Event>();
            result.Name.ShouldBe("Test");
        }

        //[Fact]
        public async Task UpdateAsyncTest()
        {
            var repo = new BaseRepository<Event>(_mockDbContext.Object, _logger.Object);

            var eventId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}");
            var newEvent = new Event
            {
                EventId = eventId,
                Name = "Test1",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            };
            var oldEvent = await _mockDbContext.Object.Set<Event>().FindAsync(eventId);

            await repo.UpdateAsync(new Event()
            {
                EventId = newEvent.EventId,
                Name = newEvent.Name,
                Price = newEvent.Price,
                Artist = newEvent.Artist,
                Date = newEvent.Date,
                Description = newEvent.Description,
                ImageUrl = newEvent.ImageUrl,
                CategoryId = newEvent.CategoryId
            });

            var events = await _mockDbContext.Object.Set<Event>().ToListAsync();
            events.Count.ShouldBe(2);
            events.ShouldContain(oldEvent);
            oldEvent.ShouldBeEquivalentTo(newEvent);
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var repo = new BaseRepository<Event>(_mockDbContext.Object, _logger.Object);
            var oldEvent = await _mockDbContext.Object.Set<Event>().FirstOrDefaultAsync();

            await repo.DeleteAsync(oldEvent);

            var events = await _mockDbContext.Object.Set<Event>().ToListAsync();
            events.ShouldNotContain(oldEvent);
            events.Count.ShouldBe(5);
        }
    }
}
