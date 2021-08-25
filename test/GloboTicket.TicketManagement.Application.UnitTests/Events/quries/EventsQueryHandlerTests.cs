using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Profiles;
using GloboTicket.TicketManagement.Application.Responses;
using GloboTicket.TicketManagement.Application.UnitTests.Mocks;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Application.UnitTests.Events.quries
{
     

    public class GetEventDetailQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<Event>> _mockEventRepository;
        private readonly Mock<IAsyncRepository<Category>> _mockCategoryRepository;
        private readonly Mock<ILogger<GetEventDetailQueryHandler>> _logger;

        public GetEventDetailQueryHandlerTest()
        {
            _mockEventRepository = RepositoryMocks.GetEventRepository();
            _mockCategoryRepository = RepositoryMocks.GetCategoryRepository();
            _logger = new Mock<ILogger<GetEventDetailQueryHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task GetEventListTest()
        {
            var handler = new GetEventDetailQueryHandler(_mapper, _mockEventRepository.Object, _mockCategoryRepository.Object);

            var eventdetail = new GetEventDetailQuery();
            eventdetail.Id = new Guid("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE");
            var result = await handler.Handle(eventdetail, CancellationToken.None);

            result.ShouldBeOfType<Response<IEnumerable<EventDetailVm>>>();

        }
    }
}
