using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Profiles;
using GloboTicket.TicketManagement.Application.Responses;
using GloboTicket.TicketManagement.Application.UnitTests.Mocks;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.AspNetCore.DataProtection;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace GloboTicket.TicketManagement.Application.UnitTests.Events.Queries
{
    public class GetEventDetailQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
 
        Mock<IDataProtectionProvider> mockDataProtectionProvider = new Mock<IDataProtectionProvider>();
        public GetEventDetailQueryHandlerTests()
        {
            _mockEventRepository = EventRepositoryMocks.GetEventRepository();
            _mockCategoryRepository = CategoryRepositoryMocks.GetCategoryRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_GetEventDetail_FromEventsRepo()
        {
            var eventId = _mockEventRepository.Object.ListAllAsync().Result.FirstOrDefault().EventId;
            CreateDataProtector(eventId.ToString());
            
            var handler = new GetEventDetailQueryHandler(_mapper, _mockEventRepository.Object, _mockCategoryRepository.Object, mockDataProtectionProvider.Object);
            var result = await handler.Handle(new GetEventDetailQuery() { Id = eventId.ToString() }, CancellationToken.None);

            result.ShouldBeOfType<Response<EventDetailVm>>();
        }

        [Fact]
        public async Task Handle_Category_NotFound()
        {
            var @event = _mockEventRepository.Object.ListAllAsync().Result.Skip(1).FirstOrDefault();
            CreateDataProtector(@event.EventId.ToString());

            var handler = new GetEventDetailQueryHandler(_mapper, _mockEventRepository.Object, _mockCategoryRepository.Object, mockDataProtectionProvider.Object);

            var result = await Should.ThrowAsync<Exceptions.NotFoundException>(() => handler.Handle(new GetEventDetailQuery() { Id = @event.EventId.ToString() }, CancellationToken.None));
            result.Message.ToLower().ShouldBe($"category ({@event.CategoryId.ToString().ToLower()}) is not found");
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
