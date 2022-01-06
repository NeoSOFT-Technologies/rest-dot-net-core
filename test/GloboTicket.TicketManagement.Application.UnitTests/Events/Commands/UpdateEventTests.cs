using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboTicket.TicketManagement.Application.Profiles;
using GloboTicket.TicketManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Application.UnitTests.Events.Commands
{
    public class UpdateEventTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<IMessageRepository> _mockMessageRepository;

        public UpdateEventTests()
        {
            _mockEventRepository = EventRepositoryMocks.GetEventRepository();
            _mockMessageRepository = MessageRepositoryMocks.GetMessageRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidEvent_UpdatedEvent()
        {
            var handler = new UpdateEventCommandHandler(_mapper, _mockEventRepository.Object, _mockMessageRepository.Object);
            var eventId = new string("61d69c462686f643ccba5033");
            var newEvent = new Domain.Entities.Event
            {
                Id = eventId,
                Name = "Test1",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = new string("61d6975072b43a1aa04fa2cb")
            };
            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);

            await handler.Handle(new UpdateEventCommand()
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Price = newEvent.Price,
                Artist = newEvent.Artist,
                Date = newEvent.Date,
                Description = newEvent.Description,
                ImageUrl = newEvent.ImageUrl,
                CategoryId = newEvent.CategoryId
            }, CancellationToken.None);

            var allEvents = await _mockEventRepository.Object.ListAllAsync();
            allEvents.Count.ShouldBe(2);
            allEvents.ShouldContain(oldEvent);
            oldEvent.ShouldBeEquivalentTo(newEvent);
        }

        [Fact]
        public async Task Handle_EmptyEvent_UpdatedEvent()
        {
            var handler = new UpdateEventCommandHandler(_mapper, _mockEventRepository.Object, _mockMessageRepository.Object);
            var eventId = new string("61d69c462686f643ccba5033");
            var newEvent = new Domain.Entities.Event
            {
                Id = eventId,
                Name = "",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = new string("61d6975072b43a1aa04fa2cb")
            };
            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new UpdateEventCommand()
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Price = newEvent.Price,
                Artist = newEvent.Artist,
                Date = newEvent.Date,
                Description = newEvent.Description,
                ImageUrl = newEvent.ImageUrl,
                CategoryId = newEvent.CategoryId
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("name is required.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_EventLength_GreaterThan_50_UpdatedEvent()
        {
            var handler = new UpdateEventCommandHandler(_mapper, _mockEventRepository.Object, _mockMessageRepository.Object);
            var eventId = new string("61d69c462686f643ccba5033");
            var newEvent = new Domain.Entities.Event
            {
                Id = eventId,
                Name = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = new string("61d6975072b43a1aa04fa2cb")
            };
            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new UpdateEventCommand()
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Price = newEvent.Price,
                Artist = newEvent.Artist,
                Date = newEvent.Date,
                Description = newEvent.Description,
                ImageUrl = newEvent.ImageUrl,
                CategoryId = newEvent.CategoryId
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("name must not exceed 50 characters.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_EmptyPrice_UpdatedEvent()
        {
            var handler = new UpdateEventCommandHandler(_mapper, _mockEventRepository.Object, _mockMessageRepository.Object);
            var eventId = new string("61d69c462686f643ccba5033");
            var newEvent = new Domain.Entities.Event
            {
                Id = eventId,
                Name = "Test1",
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = new string("61d6975072b43a1aa04fa2cb")
            };
            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new UpdateEventCommand()
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Artist = newEvent.Artist,
                Date = newEvent.Date,
                Description = newEvent.Description,
                ImageUrl = newEvent.ImageUrl,
                CategoryId = newEvent.CategoryId
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("price is required.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_Price_NotGreaterThan_0_UpdatedEvent()
        {
            var handler = new UpdateEventCommandHandler(_mapper, _mockEventRepository.Object, _mockMessageRepository.Object);
            var eventId =new string("61d69c462686f643ccba5033");
            var newEvent = new Domain.Entities.Event
            {
                Id = eventId,
                Name = "Test1",
                Artist = "test",
                Price = 0,
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = new string("61d6975072b43a1aa04fa2cb")
            };
            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new UpdateEventCommand()
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Artist = newEvent.Artist,
                Price = newEvent.Price,
                Date = newEvent.Date,
                Description = newEvent.Description,
                ImageUrl = newEvent.ImageUrl,
                CategoryId = newEvent.CategoryId
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[1].ToLower().ShouldBe("'price' must be greater than '0'.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_Event_NotFound()
        {
            var handler = new UpdateEventCommandHandler(_mapper, _mockEventRepository.Object, _mockMessageRepository.Object);
            var eventId = new string("61d6a2f395b02a9df7bbe957");
            var newEvent = new Domain.Entities.Event
            {
                Id = eventId,
                Name = "Test1",
                Artist = "test",
                Price = 0,
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = new string("61d6975072b43a1aa04fa2cb")
            };
            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);

            var result = await Should.ThrowAsync<Exceptions.NotFoundException>(() => handler.Handle(new UpdateEventCommand()
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Artist = newEvent.Artist,
                Price = newEvent.Price,
                Date = newEvent.Date,
                Description = newEvent.Description,
                ImageUrl = newEvent.ImageUrl,
                CategoryId = newEvent.CategoryId
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.Message.ToLower().ShouldBe($"event ({eventId.ToString().ToLower()}) is not found");
            allEvents.Count.ShouldBe(2);
        }
    }
}
