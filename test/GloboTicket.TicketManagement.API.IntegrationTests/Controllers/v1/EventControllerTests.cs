using GloboTicket.TicketManagement.Api;
using GloboTicket.TicketManagement.API.IntegrationTests.Base;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsList;
using GloboTicket.TicketManagement.Application.Responses;
using GloboTicket.TicketManagement.Domain.Entities;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Controllers.v1
{
    [Collection("Database")]
    public class EventControllerTests : IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public EventControllerTests(WebApplicationFactory<Startup> factory)
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

        //[Fact]
        public async Task Get_EventDetail_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();
            var eventList = await client.GetAsync("/api/V1/events");
            var eventListString = await eventList.Content.ReadAsStringAsync();
            var eventListResult = JsonConvert.DeserializeObject<Response<IEnumerable<EventListVm>>>(eventListString);
            var eventId = eventListResult.Data.FirstOrDefault().EventId;
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

   
        //[Fact]
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
    }
}
