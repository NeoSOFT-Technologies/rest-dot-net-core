using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using Moq;
using System;
using GloboTicket.TicketManagement.Application.UnitTests.Mocks;
using GloboTicket.TicketManagement.Application.Profiles;
using Xunit;
using System.Threading.Tasks;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using System.Threading;
using Shouldly;
using Microsoft.Extensions.Logging;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Models.Mail;

namespace GloboTicket.TicketManagement.Application.UnitTests.Event.Commands
{
    public class CreateEventTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<ILogger<CreateEventCommandHandler>> _logger;
        private readonly Mock<IMessageRepository> _mockMessageRepository;

        public CreateEventTests()
        {
            _mockEventRepository = EventRepositoryMocks.GetEventRepository();
            _mockMessageRepository = MessageRepositoryMocks.GetMessageRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _logger = new Mock<ILogger<CreateEventCommandHandler>>();
            _mapper = configurationProvider.CreateMapper();
            _emailService = new Mock<IEmailService>();
        }

        [Fact]
        public async Task Handle_ValidEvent_EmailSent_AddedToEventRepo()
        {
            _emailService.Setup(x => x.SendEmail(It.IsAny<Email>())).ReturnsAsync(true);
            var handler = new CreateEventCommandHandler(_mapper,_mockEventRepository.Object, _emailService.Object, _logger.Object, _mockMessageRepository.Object);
            
            await handler.Handle(new CreateEventCommand()
            {
                Name = "Test",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            }, CancellationToken.None);

            var allEvents = await _mockEventRepository.Object.ListAllAsync();
            allEvents.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Handle_ValidEvent_EmailNotSent_AddedToEventRepo()
        {
            _emailService.Setup(x => x.SendEmail(It.IsAny<Email>())).ThrowsAsync(new Exception());
            var handler = new CreateEventCommandHandler(_mapper, _mockEventRepository.Object, _emailService.Object, _logger.Object, _mockMessageRepository.Object);

            await handler.Handle(new CreateEventCommand()
            {
                Name = "Test",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            }, CancellationToken.None);

            var allEvents = await _mockEventRepository.Object.ListAllAsync();
            allEvents.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Handle_EmptyEvent_AddedToEventRepo()
        {
            var handler = new CreateEventCommandHandler(_mapper, _mockEventRepository.Object, _emailService.Object, _logger.Object, _mockMessageRepository.Object);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new CreateEventCommand()
            {
                Name = "",
                Price = 1000,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("name is required.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_EventLength_GreaterThan_50_AddedToEventRepo()
        {
            var handler = new CreateEventCommandHandler(_mapper, _mockEventRepository.Object, _emailService.Object, _logger.Object, _mockMessageRepository.Object);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new CreateEventCommand()
            {
                Name = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
                Price = 1000,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("name must not exceed 50 characters.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_EmptyDate_AddedToEventRepo()
        {
            var handler = new CreateEventCommandHandler(_mapper, _mockEventRepository.Object, _emailService.Object, _logger.Object, _mockMessageRepository.Object);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new CreateEventCommand()
            {
                Name = "Test",
                Price = 1000,
                Artist = "test",
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("date is required.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_Date_SmallerThan_CurrentDate_AddedToEventRepo()
        {
            var handler = new CreateEventCommandHandler(_mapper, _mockEventRepository.Object, _emailService.Object, _logger.Object, _mockMessageRepository.Object);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new CreateEventCommand()
            {
                Name = "Test",
                Price = 1000,
                Artist = "test",
                Date = new DateTime(2020, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("'date' must be greater than '" + DateTime.Now.ToString().ToLower() + "'.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_DuplicateEvent_AddedToEventRepo()
        {
            var handler = new CreateEventCommandHandler(_mapper, _mockEventRepository.Object, _emailService.Object, _logger.Object, _mockMessageRepository.Object);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new CreateEventCommand()
            {
                Name = "To the Moon and Back",
                Price = 25,
                Artist = "test",
                Date = DateTime.Now.AddMonths(8),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("an event with the same name and date already exists.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_EmptyPrice_AddedToEventRepo()
        {
            var handler = new CreateEventCommandHandler(_mapper, _mockEventRepository.Object, _emailService.Object, _logger.Object, _mockMessageRepository.Object);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new CreateEventCommand()
            {
                Name = "Test",
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("price is required.");
            allEvents.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_Price_NotGreaterThan_0_NotAddedToEventRepo()
        {
            var handler = new CreateEventCommandHandler(_mapper, _mockEventRepository.Object, _emailService.Object, _logger.Object, _mockMessageRepository.Object);

            var result = await Should.ThrowAsync<Exceptions.ValidationException>(() => handler.Handle(new CreateEventCommand()
            {
                Name = "Test",
                Price = 0,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            result.ValdationErrors[0].ToLower().ShouldBe("price is required.");
            result.ValdationErrors[1].ToLower().ShouldBe("'price' must be greater than '0'.");
            allEvents.Count.ShouldBe(2);
        }
    }
}
