using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboTicket.TicketManagement.Application.UnitTests.Mocks;
using Microsoft.AspNetCore.DataProtection;
using Moq;
using Shouldly;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Application.UnitTests.Events.Commands
{
    public class DeleteEventTests
    {
       
        private readonly Mock<IEventRepository> _mockEventRepository;
        Mock<IDataProtectionProvider> mockDataProtectionProvider = new Mock<IDataProtectionProvider>();

        public DeleteEventTests()
        {
            _mockEventRepository = EventRepositoryMocks.GetEventRepository();
        }

        [Fact]
        public async Task Handle_Deleted_FromEventsRepo()
        {
            var eventId = _mockEventRepository.Object.ListAllAsync().Result.FirstOrDefault().EventId;
            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);
            CreateDataProtector(eventId.ToString());

            var handler = new DeleteEventCommandHandler(_mockEventRepository.Object, mockDataProtectionProvider.Object);
            await handler.Handle(new DeleteEventCommand() { EventId = eventId.ToString() }, CancellationToken.None);
            
            var allEvents = await _mockEventRepository.Object.ListAllAsync();
            allEvents.ShouldNotContain(oldEvent);
            allEvents.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_Event_NotFound()
        {
            var eventId = "ee272f8b-6096-4cb6-8625-bb4bb2d89e8c";
            CreateDataProtector(eventId.ToString());

            var handler = new DeleteEventCommandHandler(_mockEventRepository.Object, mockDataProtectionProvider.Object);
            var result = await Should.ThrowAsync<Exceptions.NotFoundException>(() => handler.Handle(new DeleteEventCommand() { EventId = eventId }, CancellationToken.None));

            var allEvents = await _mockEventRepository.Object.ListAllAsync();
            result.Message.ToLower().ShouldBe($"event ({eventId.ToString().ToLower()}) is not found");
            allEvents.Count.ShouldBe(2);
        }

        private void CreateDataProtector(string eventId)
        {
            Mock<IDataProtector> mockDataProtector = new Mock<IDataProtector>();
            mockDataProtector.Setup(sut => sut.Protect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes("CfDJ8LIWbthOAQNJnkaD5psOVPeVho-pJeI37QJSSA2Yq5iVE-zn4-NZufSPnfi_bsi_Lhy9GMvMvgukkdQC8iJb_2EDge_YBi-P--kyu3BDBF-yDYHGATAABSLEhwKw_A6fIy_qrIgJaXkiilmFgQHYJnncaCdpjtfaEYnZzQaKc7KN"));
            mockDataProtector.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(eventId));
            mockDataProtectionProvider.Setup(s => s.CreateProtector(It.IsAny<string>())).Returns(mockDataProtector.Object);
        }
    }
}
