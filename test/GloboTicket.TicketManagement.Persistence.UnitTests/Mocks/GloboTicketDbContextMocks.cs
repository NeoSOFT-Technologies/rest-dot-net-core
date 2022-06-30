using EntityFrameworkCore3Mock;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GloboTicket.TicketManagement.Persistence.UnitTests.Mocks
{
    public class GloboTicketDbContextMocks
    {
        public static Mock<GloboTicketDbContext> GetGloboTicketDbContext()
        {
            var concertGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            var musicalGuid = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
            var playGuid = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");
            var conferenceGuid = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}");

            var categories = new List<Category>
            {
                new Category
                {
                    CategoryId = concertGuid,
                    Name = "Concerts",
                    Events = new List<Event>
                    {
                        new Event
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
                        new Event
                        {
                            EventId = Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"),
                            Name = "The State of Affairs: Michael Live!",
                            Price = 85,
                            Artist = "Michael Johnson",
                            Date = DateTime.Now.AddMonths(9),
                            Description = "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
                            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/michael.jpg",
                            CategoryId = concertGuid
                        },
                        new Event
                        {
                            EventId = Guid.Parse("{B419A7CA-3321-4F38-BE8E-4D7B6A529319}"),
                            Name = "Clash of the DJs",
                            Price = 85,
                            Artist = "DJ 'The Mike'",
                            Date = DateTime.Now.AddMonths(4),
                            Description = "DJs from all over the world will compete in this epic battle for eternal fame.",
                            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/dj.jpg",
                            CategoryId = concertGuid
                        },
                        new Event
                        {
                            EventId = Guid.Parse("{62787623-4C52-43FE-B0C9-B7044FB5929B}"),
                            Name = "Spanish guitar hits with Manuel",
                            Price = 25,
                            Artist = "Manuel Santinonisi",
                            Date = DateTime.Now.AddMonths(4),
                            Description = "Get on the hype of Spanish Guitar concerts with Manuel.",
                            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/guitar.jpg",
                            CategoryId = concertGuid
                        }
                    }
                },
                new Category
                {
                    CategoryId = musicalGuid,
                    Name = "Musicals",
                    Events = new List<Event>
                    {
                        new Event
                        {
                            EventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
                            Name = "To the Moon and Back",
                            Price = 135,
                            Artist = "Nick Sailor",
                            Date = DateTime.Now.AddMonths(8),
                            Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
                            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                            CategoryId = musicalGuid
                        }
                    }
                },
                new Category
                {
                    CategoryId = conferenceGuid,
                    Name = "Conferences",
                    Events = new List<Event>
                    {
                        new Event
                        {
                            EventId = Guid.Parse("{1BABD057-E980-4CB3-9CD2-7FDD9E525668}"),
                            Name = "Techorama 2021",
                            Price = 400,
                            Artist = "Many",
                            Date = DateTime.Now.AddMonths(10),
                            Description = "The best tech conference in the world",
                            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/conf.jpg",
                            CategoryId = conferenceGuid
                        }
                    }
                },
                 new Category
                {
                    CategoryId = playGuid,
                    Name = "Plays",
                    Events = new List<Event>{}
                }
            };

            var events = new List<Event>
            {
                new Event
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
                new Event
                {
                    EventId = Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"),
                    Name = "The State of Affairs: Michael Live!",
                    Price = 85,
                    Artist = "Michael Johnson",
                    Date = new DateTime(2027, 1, 18),
                    Description = "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/michael.jpg",
                    CategoryId = concertGuid
                },
                new Event
                {
                    EventId = Guid.Parse("{B419A7CA-3321-4F38-BE8E-4D7B6A529319}"),
                    Name = "Clash of the DJs",
                    Price = 85,
                    Artist = "DJ 'The Mike'",
                    Date = DateTime.Now.AddMonths(4),
                    Description = "DJs from all over the world will compete in this epic battle for eternal fame.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/dj.jpg",
                    CategoryId = concertGuid
                },
                new Event
                {
                    EventId = Guid.Parse("{62787623-4C52-43FE-B0C9-B7044FB5929B}"),
                    Name = "Spanish guitar hits with Manuel",
                    Price = 25,
                    Artist = "Manuel Santinonisi",
                    Date = DateTime.Now.AddMonths(4),
                    Description = "Get on the hype of Spanish Guitar concerts with Manuel.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/guitar.jpg",
                    CategoryId = concertGuid
                },
                new Event
                {
                    EventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
                    Name = "To the Moon and Back",
                    Price = 135,
                    Artist = "Nick Sailor",
                    Date = DateTime.Now.AddMonths(8),
                    Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                    CategoryId = musicalGuid
                },
                new Event
                {
                    EventId = Guid.Parse("{1BABD057-E980-4CB3-9CD2-7FDD9E525668}"),
                    Name = "Techorama 2021",
                    Price = 400,
                    Artist = "Many",
                    Date = DateTime.Now.AddMonths(10),
                    Description = "The best tech conference in the world",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/conf.jpg",
                    CategoryId = conferenceGuid
                }
            };

            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.Parse("{7E94BC5B-71A5-4C8C-BC3B-71BB7976237E}"),
                    OrderTotal = 400,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = Guid.Parse("{A441EB40-9636-4EE6-BE49-A66C5EC1330B}")
                },
                new Order
                {
                    Id = Guid.Parse("{86D3A045-B42D-4854-8150-D6A374948B6E}"),
                    OrderTotal = 135,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = Guid.Parse("{AC3CFAF5-34FD-4E4D-BC04-AD1083DDC340}")
                },
                new Order
                {
                    Id = Guid.Parse("{771CCA4B-066C-4AC7-B3DF-4D12837FE7E0}"),
                    OrderTotal = 85,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = Guid.Parse("{D97A15FC-0D32-41C6-9DDF-62F0735C4C1C}")
                },
                new Order
                {
                    Id = Guid.Parse("{3DCB3EA0-80B1-4781-B5C0-4D85C41E55A6}"),
                    OrderTotal = 245,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = Guid.Parse("{4AD901BE-F447-46DD-BCF7-DBE401AFA203}")
                },
                new Order
                {
                    Id = Guid.Parse("{E6A2679C-79A3-4EF1-A478-6F4C91B405B6}"),
                    OrderTotal = 142,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = Guid.Parse("{7AEB2C01-FE8E-4B84-A5BA-330BDF950F5C}")
                },
                new Order
                {
                    Id = Guid.Parse("{F5A6A3A0-4227-4973-ABB5-A63FBE725923}"),
                    OrderTotal = 40,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = Guid.Parse("{F5A6A3A0-4227-4973-ABB5-A63FBE725923}")
                },
                new Order
                {
                    Id = Guid.Parse("{BA0EB0EF-B69B-46FD-B8E2-41B4178AE7CB}"),
                    OrderTotal = 116,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = Guid.Parse("{7AEB2C01-FE8E-4B84-A5BA-330BDF950F5C}")
                }
            };

            var messages = new List<Message>
            {
                new Message
                {
                    MessageId = Guid.NewGuid(),
                    Code = "1",
                    MessageContent = "{PropertyName} is required.",
                    Language = "en",
                    Type = Message.MessageType.Error
                },
                new Message
                {
                    MessageId = Guid.NewGuid(),
                    Code = "2",
                    MessageContent = "{PropertyName} must not exceed {MaxLength} characters.",
                    Language = "en",
                    Type = Message.MessageType.Error
                },
                new Message
                {
                    MessageId = Guid.NewGuid(),
                    Code = "3",
                    MessageContent = "An event with the same name and date already exists.",
                    Language = "en",
                    Type = Message.MessageType.Error
                }
            };

            var mockGloboTicketDbContext = new DbContextMock<GloboTicketDbContext>(new DbContextOptionsBuilder<GloboTicketDbContext>().Options);

            var mockCategoriessSet = categories.AsQueryable().BuildMockDbSet();
            var mockEventsSet = events.AsQueryable().BuildMockDbSet();
            var mockOrdersSet = orders.AsQueryable().BuildMockDbSet();
            var mockMessagesSet = messages.AsQueryable().BuildMockDbSet();

            mockGloboTicketDbContext.Setup(ctx => ctx.Set<Category>()).Returns(mockCategoriessSet.Object);
            mockGloboTicketDbContext.Setup(ctx => ctx.Set<Event>()).Returns(mockEventsSet.Object);
            mockGloboTicketDbContext.Setup(ctx => ctx.Set<Order>()).Returns(mockOrdersSet.Object);
            mockGloboTicketDbContext.Setup(ctx => ctx.Set<Message>()).Returns(mockMessagesSet.Object);

            mockCategoriessSet.Setup(set => set.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>())).Callback(
                (Category category, CancellationToken token) =>
                {
                    categories.Add(category);
                });

            mockEventsSet.Setup(set => set.FindAsync(It.IsAny<Guid>())).ReturnsAsync(
                (object[] ids) =>
                {
                    var id = (Guid)ids[0];
                    return events.FirstOrDefault(x => x.EventId == id);
                });
            mockEventsSet.Setup(set => set.AddAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>())).Callback(
                (Event @event, CancellationToken token) =>
                {
                    events.Add(@event);
                });
            mockEventsSet.Setup(set => set.Remove(It.IsAny<Event>())).Callback(
                (Event @event) =>
                {
                    events.Remove(@event);
                });

            return mockGloboTicketDbContext;
        }
    }
}
