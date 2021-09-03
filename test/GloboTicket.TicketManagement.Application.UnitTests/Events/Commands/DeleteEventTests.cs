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
        public async Task Handle_Deleted_FromEventsRepo()
        {
            var handler = new DeleteEventCommandHandler(_mapper, _mockEventRepository.Object);
            var eventId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}");

            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);
            await handler.Handle(new DeleteEventCommand() { EventId = eventId }, CancellationToken.None);
            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            allEvents.ShouldNotContain(oldEvent);
            allEvents.Count.ShouldBe(1);
        }
    }
}
