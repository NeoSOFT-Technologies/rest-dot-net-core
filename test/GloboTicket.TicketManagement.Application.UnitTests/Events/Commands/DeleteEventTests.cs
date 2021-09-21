using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboTicket.TicketManagement.Application.Profiles;
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
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mock<IDataProtector> mockDataProtector = new Mock<IDataProtector>();
            mockDataProtector.Setup(sut => sut.Protect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes("CfDJ8LIWbthOAQNJnkaD5psOVPeVho-pJeI37QJSSA2Yq5iVE-zn4-NZufSPnfi_bsi_Lhy9GMvMvgukkdQC8iJb_2EDge_YBi-P--kyu3BDBF-yDYHGATAABSLEhwKw_A6fIy_qrIgJaXkiilmFgQHYJnncaCdpjtfaEYnZzQaKc7KN"));
            mockDataProtector.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes("ee272f8b-6096-4cb6-8625-bb4bb2d89e8b"));
            mockDataProtectionProvider.Setup(s => s.CreateProtector(It.IsAny<string>())).Returns(mockDataProtector.Object);

        }

        [Fact]
        public async Task Handle_Deleted_FromEventsRepo()
        {
            var handler = new DeleteEventCommandHandler(_mockEventRepository.Object, mockDataProtectionProvider.Object);
            var eventId = _mockEventRepository.Object.ListAllAsync().Result.FirstOrDefault().EventId;

            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(eventId);
            await handler.Handle(new DeleteEventCommand() { EventId = eventId.ToString() }, CancellationToken.None);
            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            allEvents.ShouldNotContain(oldEvent);
            allEvents.Count.ShouldBe(1);
        }
    }
}
