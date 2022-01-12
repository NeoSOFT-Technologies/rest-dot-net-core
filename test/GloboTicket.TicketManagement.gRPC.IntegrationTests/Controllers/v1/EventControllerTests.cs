using GloboTicket.TicketManagement.gRPC.IntegrationTests.Base;
using GrpcEventClient;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
//using static GrpcEventClient.EventProtoService;

namespace GloboTicket.TicketManagement.gRPC.IntegrationTests.Controllers.v1
{
    [Collection("Database")]
    public class EventControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly EventProtoService.EventProtoServiceClient _client;
        public EventControllerTests(CustomWebApplicationFactory factory)
        {
            _client = new EventProtoService.EventProtoServiceClient(factory.channel);
        }

        [Fact]
        public async Task Get_EventsList_ReturnsSuccessResult()
        {
            EventModelList response = await _client.GetAllEventAsync(new AllEventRequest());
            response.Eventmodel.ShouldNotBeEmpty();
            response.ShouldBeOfType<EventModelList>();
        }

        [Fact]
        public async Task Get_EventDetail_ReturnsSuccessResult()
        {
            string id = await GetFirstEventId();
            var response = await _client.GetEventByIdAsync(new EventRequest() { Id = id });
            response.ShouldNotBeNull();
            response.ShouldBeOfType<EventModel>();
        }

        [Fact]
        public async Task Post_Event_ReturnsSuccessResult()
        {
            CreateEventRequest request = new CreateEventRequest()
            {
                Name = "Test Name",
                Price = 75,
                Artist = "Test Artist",
                Date = DateTime.UtcNow.AddMonths(6).ToString(),
                Description = "Test Description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",
                CategoryId = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}").ToString()
            };

            var response = await _client.CreateEventAsync(request);
            response.Succeeded.ShouldBeEquivalentTo(true);
            response.ShouldBeOfType<EventModelReturn>();
        }

        [Fact]
        public async Task Put_Event_ReturnsSuccessResult()
        {
            UpdateEventRequest request = new UpdateEventRequest()
            {
                EventId = Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}").ToString(),
                Name = "Test Name1",
                Price = 75,
                Artist = "Test Artist",
                Date = DateTime.UtcNow.AddMonths(6).ToString(),
                Description = "Test Description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",
                CategoryId = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}").ToString()
            };

            var response = await _client.UpdateEventAsync(request);
            response.ShouldNotBeNull();
            response.ShouldBeOfType<EventModelReturn>();
        }

        [Fact]
        public async Task Delete_Event_ReturnsNoContentResult()
        {
            string id = await GetFirstEventId();
            var response = await _client.DeleteEventAsync(new DeleteEventRequest() { EventId=id});
            response.ShouldNotBeNull();
            response.ShouldBeOfType<DeleteEventReturn>();
        }

        internal async Task<string> GetFirstEventId()
        {
            var response = await _client.GetAllEventAsync(new AllEventRequest());
        return response.Eventmodel.FirstOrDefault().EventId;

    }
    }
}

