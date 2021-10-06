using GloboTicket.TicketManagement.API.IntegrationTests.Base;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsList;
using GloboTicket.TicketManagement.Application.Responses;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.Transaction;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Controllers.v1
{
    [Collection("Database")]
    public class EventControllerTests : IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _factory;

        public EventControllerTests(WebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_EventsList_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/V1/events");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<IEnumerable<EventListVm>>>(responseString);
            result.Data.ShouldBeOfType<List<EventListVm>>();
            result.Data.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task Get_EventDetail_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();
            var eventId = await GetFirstEventId();
            var response = await client.GetAsync("/api/V1/events/" + eventId);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<EventDetailVm>>(responseString);
            result.Data.ShouldBeOfType<EventDetailVm>();
            result.Data.ShouldNotBeNull();
        }

        //[Fact]
        //public async Task Get_EventsExport_ReturnsSuccessResult()
        //{
        //    var client = _factory.GetAnonymousClient();

        //    var response = await client.GetAsync("/api/V1/events/export");

        //    response.EnsureSuccessStatusCode();

        //    var responseString = await response.Content.ReadAsStringAsync();

        //    var result = JsonConvert.DeserializeObject<EventExportFileVm>(responseString);

        //    result.ShouldBeOfType<List<EventListVm>>();
        //    result.ShouldNotBeNull();
        //}

   
        [Fact]
        public async Task Post_Event_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();
            var @event = new CreateEventCommand()
            {
                Name = "Test Name",
                Price = 75,
                Artist = "Test Artist",
                Date = DateTime.Now.AddMonths(6),
                Description = "Test Description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",
                CategoryId = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}")
            };
            var eventJson = JsonConvert.SerializeObject(@event);
            HttpContent content = new StringContent(eventJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/events", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<Guid>>(responseString);
            result.Succeeded.ShouldBeEquivalentTo(true);
            result.Data.ShouldBeOfType<Guid>();
            result.Errors.ShouldBeNull();
        }

        [Fact]
        public async Task Post_Event_Transaction_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();
            var @event = new TransactionCommand()
            {
                Name = "Test Name1",
                Price = 75,
                Artist = "Test Artist",
                Date = DateTime.Now.AddMonths(7),
                Description = "Test Description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",
                CategoryName = "TestCat"
            };
            var eventJson = JsonConvert.SerializeObject(@event);
            HttpContent content = new StringContent(eventJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/events/transactiondemo", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<Guid>>(responseString);
            result.Succeeded.ShouldBeEquivalentTo(true);
            result.Data.ShouldBeOfType<Guid>();
            result.Errors.ShouldBeNull();
        }

        [Fact]
        public async Task Put_Event_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();
            var @event = new UpdateEventCommand()
            {
                EventId = Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"),
                Name = "Test Name1",
                Price = 75,
                Artist = "Test Artist",
                Date = DateTime.Now.AddMonths(6),
                Description = "Test Description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",
                CategoryId = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}")
            };
            var eventJson = JsonConvert.SerializeObject(@event);
            HttpContent content = new StringContent(eventJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/v1/events", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<Guid>>(responseString);
            result.Succeeded.ShouldBeEquivalentTo(true);
            result.Data.ShouldBeOfType<Guid>();
            result.Errors.ShouldBeNull();
        }

        [Fact]
        public async Task Delete_Event_ReturnsNoContentResult()
        {
            var client = _factory.CreateClient();
            var eventId = await GetFirstEventId();
            var response = await client.DeleteAsync("/api/V1/events/" + eventId);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
        }

        private async Task<string> GetFirstEventId()
        {
            var client = _factory.CreateClient();
            var eventList = await client.GetAsync("/api/V1/events");
            var eventListString = await eventList.Content.ReadAsStringAsync();
            var eventListResult = JsonConvert.DeserializeObject<Response<IEnumerable<EventListVm>>>(eventListString);
            return eventListResult.Data.FirstOrDefault().EventId;
        }
    }
}
