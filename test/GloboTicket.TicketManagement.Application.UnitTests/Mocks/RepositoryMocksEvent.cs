using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Application.UnitTests.Mocks
{
    public class RepositoryMocksEvent
    {
        public static Mock<IEventRepository> GetEventRepository()
        {
            
            var EventList = new List<GloboTicket.TicketManagement.Domain.Entities.Event>
            {
                new Domain.Entities.Event
                {
                    EventId = Guid.Parse("ADC42C09-08C1-4D2C-9F96-2D15BB1AF299"),
                    //Name = "To the Moon and Back",
                    //Price=25,
                    //Artist = "Nick Sailor",
                    //Date=DateTime.Now,
                    //Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
                    //ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                    //CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
                Name = "Test",
                Price = 25,
                Artist = "test",
                Date = new DateTime(2027, 1, 18),
                Description = "description",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
                }
                //new Domain.Entities.Event
                //{
                //    EventId = Guid.Parse("{B419A7CA-3321-4F38-BE8E-4D7B6A529319}"),
                //    Name = "Clash of the DJs",
                //    Artist = "DJ 'The Mike'",
                //    Description = "DJs from all over the world will compete in this epic battle for eternal fame.",
                //    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                //    CategoryId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}")
                //}

            };

            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(EventList);
            mockEventRepository.Setup(repo => repo.GetByIdAsync(Guid.Parse("ADC42C09-08C1-4D2C-9F96-2D15BB1AF299")));

            mockEventRepository.Setup(repo => repo.AddAsync(It.IsAny<GloboTicket.TicketManagement.Domain.Entities.Event>())).ReturnsAsync(
                (GloboTicket.TicketManagement.Domain.Entities.Event Event) =>
                {
                    EventList.Add(Event);
                    return Event;
                });

            return mockEventRepository;

        }
    }
}
