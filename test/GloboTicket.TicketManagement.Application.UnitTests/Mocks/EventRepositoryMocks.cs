using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GloboTicket.TicketManagement.Application.UnitTests.Mocks
{
    public class EventRepositoryMocks
    {
        public static Mock<IEventRepository> GetEventRepository()
        {
            var concertGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            var musicalGuid = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
            var EventList = new List<Domain.Entities.Event>
            {
                new Domain.Entities.Event
                {
                    EventId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                    Name = "John Egbert Live",
                    Price = 65,
                    Artist = "John Egbert",
                    Date = DateTime.Now.AddMonths(6),
                    Description = "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",
                    CategoryId = concertGuid
                },
                new Domain.Entities.Event
                {
                    EventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
                    Name = "To the Moon and Back",
                    Price = 135,
                    Artist = "Nick Sailor",
                    Date = DateTime.Now.AddMonths(8),
                    Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                    CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A9}")
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
            mockEventRepository.Setup(repo => repo.IsEventNameAndDateUnique(It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(
                (string name, DateTime date) =>
                {
                    var matches = EventList.Any(e => e.Name.Equals(name) && e.Date.Date.Equals(date.Date));
                    return matches;
                });

            mockEventRepository.Setup(repo => repo.AddEventWithCategory(It.IsAny<Domain.Entities.Event>())).ReturnsAsync(
                (Domain.Entities.Event Event) =>
                {
                    EventList.Add(Event);
                    return Event;
                });

            return mockEventRepository;

        }
    }
}
