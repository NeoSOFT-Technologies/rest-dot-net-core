using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Application.UnitTests.Mocks;
using GloboTicket.TicketManagement.Application.Profiles;
using Xunit;
using System.Threading.Tasks;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using System.Threading;
using Shouldly;
using Microsoft.Extensions.Logging;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;

namespace GloboTicket.TicketManagement.Application.UnitTests.Event.Commands
{
    public class CreateEventTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEventRepository> _mockEventRepository;

        private readonly IEventRepository _eventRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<CreateEventCommandHandler> _logger;

        public CreateEventTests()
        {
            _mockEventRepository = RepositoryMocksEvent.GetEventRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidEvent_AddedToEventRepo1()
        {
            var handler = new CreateEventCommandHandler(_mapper,_mockEventRepository.Object, _emailService,_logger);

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
            allEvents.Count.ShouldBe(2);
        }

    }
}
