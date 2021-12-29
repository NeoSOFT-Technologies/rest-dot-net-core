using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Persistence
{
    public class ContextSeedData<T> where T: class, IDocument
    {
        public static void SeedData(IMongoCollection<T> dataCollection)
        {
            //check if there is any existing product collections
            bool existProduct = dataCollection.Find(p => true).Any();
            if (!existProduct)
            {
                dataCollection.InsertManyAsync(GetPreconfiguredProducts());

            }
        }
        private static IEnumerable<Category> GetPreconfiguredProducts()
        {
            var concertGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            var musicalGuid = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
            var playGuid = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");
            var conferenceGuid = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}");

            return new List<Category>()
            {
                new Category()
                {
                    Id = concertGuid,
                    Name = "Concerts"
                }
            };
        }

        private static IEnumerable<Event> GetPreconfiguredProducts()
        {
            var concertGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            var musicalGuid = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
            var playGuid = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");
            var conferenceGuid = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}");

            return new List<Event>()
            {
                new Event()
                {
                    Id = concertGuid,
                    Name = "Concerts"
                }
            };
        }
    }
}
