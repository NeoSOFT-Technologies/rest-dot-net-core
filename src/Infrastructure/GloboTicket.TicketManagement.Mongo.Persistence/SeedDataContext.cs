using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace GloboTicket.TicketManagement.Mongo.Persistence
{
    public class SeedDataContext
    {
        public static void SeedData(IMongoDbSettings dbSettings)
        {
            var Context1 = new MongoClient(dbSettings.ConnectionString).GetDatabase(dbSettings.DatabaseName);
            SeedCategory(Context1.GetCollection<Category>("Category"));
            SeedMessage(Context1.GetCollection<Message>("Message"));
            SeedEvent(Context1.GetCollection<Event>("Event"));
            SeedOrder(Context1.GetCollection<Order>("Order"));
        }


        private static void SeedCategory(IMongoCollection<Category> dataCollection)
        {
            var concertGuid = new string("61cc58c88b8879cc049839a8");
            var musicalGuid = new string("61d1cc89ace80d15f6249a53");

            //check if there is any existing product collections
            bool dataExist = dataCollection.Find(p => true).Any();
            if (!dataExist)
            {
                List<Category> category = new List<Category>()
                {
                new Category()
                {
                    Id = concertGuid,
                    Name = "Concerts"
                },
                new Category()
                {
                    Id = musicalGuid,
                    Name = "Musicals"
                }
                };
                dataCollection.InsertMany(category);

            }
        }

        private static void SeedEvent(IMongoCollection<Event> dataCollection)
        {
            var concertGuid = new string("61cc58c88b8879cc049839a8");
            var musicalGuid = new string("61d1cc89ace80d15f6249a53");

            bool dataExist = dataCollection.Find(p => true).Any();
            if (!dataExist)
            {
                List<Event> events = new List<Event>()
                {
                new Event()
                {
                    Id =new string("61cc69c07753322250b9307b"),
                   Name = "John Egbert Live",
                    Price = 65,
                    Artist = "John Egbert",
                    Date = DateTime.Now.AddMonths(6),
                    Description = "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",
                    CategoryId = concertGuid
                },
                 new Event()
                {
                     Id = new string("61d1cd26ed5d461680c1f708"),
                    Name = "To the Moon and Back",
                    Price = 135,
                    Artist = "Nick Sailor",
                    Date = DateTime.Now.AddMonths(8),
                    Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                    CategoryId = musicalGuid
                },
                  new Event()
                {
                     Id = new string("61d1cd26ed5d461680c1f710"),
                    Name = "To the Moon",
                    Price = 435,
                    Artist = "Nick Sailor",
                    Date = DateTime.Now.AddMonths(8),
                    Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                    CategoryId = musicalGuid
                }
                };
                dataCollection.InsertMany(events);

            }
        }



        private static void SeedOrder(IMongoCollection<Order> dataCollection)
        {
            bool dataExist = dataCollection.Find(p => true).Any();
            if (!dataExist)
            {
                List<Order> orders = new List<Order>()
                {
                new Order()
                {
                    Id= new string("61cc69962ff50bdbbb0ef1f7"),
                    OrderTotal = 400,
                    OrderPaid = true,
                    OrderPlaced = DateTime.Now
                },
                new Order()
                {
                    Id= ObjectId.GenerateNewId().ToString(),
                    OrderTotal = 2400,
                    OrderPaid = true,
                    OrderPlaced = DateTime.Now
                },
                new Order()
                {
                    Id= ObjectId.GenerateNewId().ToString(),
                    OrderTotal = 100,
                    OrderPaid = true,
                    OrderPlaced = DateTime.Now
                },
                new Order()
                {
                    Id= ObjectId.GenerateNewId().ToString(),
                    OrderTotal = 300,
                    OrderPaid = true,
                    OrderPlaced = DateTime.Now
                },
                new Order()
                {
                    Id= ObjectId.GenerateNewId().ToString(),
                    OrderTotal = 2100,
                    OrderPaid = true,
                    OrderPlaced = DateTime.Now
                },
                new Order()
                {
                    Id= ObjectId.GenerateNewId().ToString(),
                    OrderTotal = 4500,
                    OrderPaid = true,
                    OrderPlaced = DateTime.Now.AddMonths(8)
                }
                };
                dataCollection.InsertMany(orders);

            }
        }

        private static void SeedMessage(IMongoCollection<Message> dataCollection)
        {
            bool dataExist = dataCollection.Find(p => true).Any();
            if (!dataExist)
            {
            List<Message> messages = new List<Message>()
            {
            new Message()
            {
                Id= new string("61cc69962ff50bdbbb0ef1f7"),
                Code = "1",
                MessageContent = "{PropertyName} is required.",
                Language = "en",
                Type = Message.MessageType.Error
            },
            new Message()
            {
                Id= new string("61cc6987cd972a2af94a8e77"),
                Code = "2",
                MessageContent = "{PropertyName} must not exceed {MaxLength} characters.",
                Language = "en",
                Type = Message.MessageType.Error

            },
            new Message()
            {
                Id= new string("61cc697b02083af977eaff3f"),
                Code = "3",
                MessageContent = "An event with the same name and date already exists.",
                Language = "en",
                Type = Message.MessageType.Error
            }
                };
                dataCollection.InsertMany(messages);

            }
        }
    }
}
