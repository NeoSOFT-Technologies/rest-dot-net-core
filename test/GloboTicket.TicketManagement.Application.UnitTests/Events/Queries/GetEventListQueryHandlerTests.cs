using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsList;
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

namespace GloboTicket.TicketManagement.Application.UnitTests.Event.Queries
{
    public class GetEventListQueryHandlerTests
    {
        private readonly IMapper _mapper;
        //private readonly Mock<IAsyncRepository<GloboTicket.TicketManagement.Domain.Entities.Event>> _mockEventRepository;
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly ILogger<GetEventsListQueryHandler> _logger;
        //private readonly ILogger<CreateEventCommandHandler> _logger;
        private readonly Mock<IAsyncRepository<Category>> _mockCategoryRepository;

        public GetEventListQueryHandlerTests()
        {
            _mockEventRepository = EventRepositoryMocks.GetEventRepository();
            _mockCategoryRepository = CategoryRepositoryMocks.GetCategoryRepository();
            //_logger = new Mock<ILogger<GetEventListQueryHandler>>();
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
            _mockEventRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new GloboTicket.TicketManagement.Domain.Entities.Event
            {
                EventId = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}"),
                Name = "Concerts"
            });
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Category
            {
                CategoryId = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}"),
                Name = "Concerts"
            });
            var result = await handler.Handle(eventdetail, CancellationToken.None);

            result.ShouldBeOfType<Response<EventDetailVm>>();

        }
    }
}
