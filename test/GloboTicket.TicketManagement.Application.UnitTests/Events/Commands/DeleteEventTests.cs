using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
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
    public class DeleteEventTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEventRepository> _mockEventRepository;

        public DeleteEventTests()
        {
            _mockEventRepository = EventRepositoryMocks.GetEventRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_DeletedFromEventsRepo()
        {
            var handler = new DeleteEventCommandHandler(_mapper, _mockEventRepository.Object);
            var eventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}");

            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);
            await handler.Handle(new DeleteEventCommand() { EventId = eventId }, CancellationToken.None);
            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            allEvents.ShouldNotContain(oldEvent);
            allEvents.Count.ShouldBe(0);
        }
    }
}
