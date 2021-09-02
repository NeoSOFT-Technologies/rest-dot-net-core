using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GloboTicket.TicketManagement.Application.UnitTests.Mocks
{
    public class EventRepositoryMocks
    {
        public static Mock<IEventRepository> GetEventRepository()
        {

            var EventList = new List<Domain.Entities.Event>
            {
                new Domain.Entities.Event
                {
                    EventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
                    Name = "Test",
                    Price = 25,
                    Artist = "test",
                    Date = new DateTime(2027, 1, 18),
                    Description = "description",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                    CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
                }
            };

            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(EventList);
            mockEventRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                (Guid EventId) =>
                {
                    return EventList.SingleOrDefault(x => x.EventId == EventId);
                });
            mockEventRepository.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Event>())).ReturnsAsync(
                (Domain.Entities.Event Event) =>
                {
                    EventList.Add(Event);
                    return Event;
                });
            mockEventRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Event>())).Callback(
                (Domain.Entities.Event Event) =>
                {
                    EventList.Remove(Event);
                });
            mockEventRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Event>())).Callback(
                (Domain.Entities.Event Event) =>
                {
                    var oldEvent = EventList.First(x => x.EventId == Event.EventId);
                    var index = EventList.IndexOf(oldEvent);
                    if (index != -1)
                        EventList[index] = Event;
                });

            return mockEventRepository;

        }
    }
}
