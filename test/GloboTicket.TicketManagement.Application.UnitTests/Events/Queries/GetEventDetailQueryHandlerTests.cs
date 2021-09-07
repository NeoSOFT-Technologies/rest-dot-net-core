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
            var handler = new GetEventDetailQueryHandler(_mapper, _mockEventRepository.Object, _mockCategoryRepository.Object);
            var eventId = _mockEventRepository.Object.ListAllAsync().Result.FirstOrDefault().EventId;
            var dataProtectionProvider = DataProtectionProvider.Create("Test");
            var protector = dataProtectionProvider.CreateProtector("Test");
            string id = protector.Protect(eventId.ToString());

            var result = await handler.Handle(new GetEventDetailQuery() { Id = id }, CancellationToken.None);

            result.ShouldBeOfType<Response<EventDetailVm>>();
        }
    }
}
