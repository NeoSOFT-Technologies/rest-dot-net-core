﻿using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboTicket.TicketManagement.Application.Profiles;
using GloboTicket.TicketManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Application.UnitTests.Events.Commands
{
    public class UpdateEventTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEventRepository> _mockEventRepository;

        public UpdateEventTests()
        {
            _mockEventRepository = EventRepositoryMocks.GetEventRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidEvent_UpdatedEvent()
        {
            var handler = new UpdateEventCommandHandler(_mapper, _mockEventRepository.Object);
            var newEvent = new Domain.Entities.Event
            {
                EventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
                Name = "Test1",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
            };

            var oldEvent = await _mockEventRepository.Object.GetByIdAsync(Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"));
            await handler.Handle(new UpdateEventCommand
            {
                EventId = newEvent.EventId,
                Name = newEvent.Name,
                Price = newEvent.Price,
                Artist = newEvent.Artist,
                Date = newEvent.Date,
                Description = newEvent.Description,
                ImageUrl = newEvent.ImageUrl,
                CategoryId = newEvent.CategoryId
            }, CancellationToken.None);
            var allEvents = await _mockEventRepository.Object.ListAllAsync();

            allEvents.Count.ShouldBe(1);
            allEvents.ShouldContain(oldEvent);
            oldEvent.ShouldBeEquivalentTo(newEvent);
        }
    }
}
