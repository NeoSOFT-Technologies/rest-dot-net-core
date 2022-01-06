using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GloboTicket.TicketManagement.Application.UnitTests.Mocks
{
    public class CategoryRepositoryMocks
    {
        public static Mock<ICategoryRepository> GetCategoryRepository()
        {
            /* var concertGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
             var musicalGuid = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
             var playGuid = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");
             var conferenceGuid = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}");*/

            var concertGuid =new string("61d6975072b43a1aa04fa2cb");
            var musicalGuid = new string("61d69763654c680ecb58462f");
            var playGuid = new string("61d69788241b62ae435fbe64");
            var conferenceGuid = new string("61d69793332ee3121004da32");

            var categories = new List<Category>
            {
                new Category
                {
                    Id = concertGuid,
                    Name = "Concerts",
                    Events = new List<Domain.Entities.Event>
                    {
                        new Domain.Entities.Event
                        {
                            Id = new string("61d697d93571de6e6106ebca"),
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
                           Id = new string("61d6980680ccbe45037035d9") ,
                            Name = "The State of Affairs: Michael Live!",
                            Price = 85,
                            Artist = "Michael Johnson",
                            Date = DateTime.Now.AddMonths(9),
                            Description = "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
                            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/michael.jpg",
                            CategoryId = concertGuid
                        },
                        new Domain.Entities.Event
                        {
                            Id = new string("61d69842044ac4c8d1dedaca") ,
                            Name = "Clash of the DJs",
                            Price = 85,
                            Artist = "DJ 'The Mike'",
                            Date = DateTime.Now.AddMonths(4),
                            Description = "DJs from all over the world will compete in this epic battle for eternal fame.",
                            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/dj.jpg",
                            CategoryId = concertGuid
                        },
                        new Domain.Entities.Event
                        {
                            Id =  new string("61d6984cb7512b6e8b95919d"),
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
                    Id = musicalGuid,
                    Name = "Musicals",
                    Events = new List<Domain.Entities.Event>
                    {
                        new Domain.Entities.Event
                        {
                            Id =  new string("61d6987686adda6d83486a9b"),
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
                    Id = conferenceGuid,
                    Name = "Conferences",
                    Events = new List<Domain.Entities.Event>
                    {
                        new Domain.Entities.Event
                        {
                            Id = new string("61d698ebf837da8b2204164f") ,
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
                    Id = playGuid,
                    Name = "Plays",
                    Events = new List<Domain.Entities.Event>{}
                }
            };

            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(categories);
            mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(
                (string CategoryId) =>
                {
                    return categories.SingleOrDefault(x => x.Id == CategoryId);
                });
            mockCategoryRepository.Setup(repo => repo.GetCategoriesWithEvents(It.IsAny<bool>())).ReturnsAsync((bool includePassedEvents) =>
            {
                if (!includePassedEvents)
                {
                    categories.ForEach(p => p.Events.ToList().RemoveAll(c => c.Date < DateTime.Today));
                    return categories;
                }
                return categories;
            });
            mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>())).ReturnsAsync(
                (Category category) =>
                {
                    categories.Add(category);
                    return category;
                });
            mockCategoryRepository.Setup(repo => repo.AddCategory(It.IsAny<Category>())).ReturnsAsync(
                (Category category) =>
                {
                    category.Id = ObjectId.GenerateNewId().ToString()/* Guid.NewGuid()*/;
                    categories.Add(category);
                    return category;
                });

            return mockCategoryRepository;
        }
        
    }
}
