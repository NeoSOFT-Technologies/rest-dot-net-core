using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
/*using Microsoft.EntityFrameworkCore;*/
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly string cacheKey = $"{typeof(Message)}";
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public MessageRepository(/*GloboTicketDbContext*/ /*IMongoDbContext*/IMongoDbSettings dbContext, ILogger<Message> logger, ICacheService cacheService) : base(dbContext, logger)
        {
            _logger = logger;
            _cacheService = cacheService;

            SeedData(_dbContext);
        }
        public static void SeedData(IMongoCollection<Message> dataCollection)
        {
            //check if there is any existing product collections
            bool existProduct = dataCollection.Find(p => true).Any();
            if (!existProduct)
            {
                dataCollection.InsertManyAsync(GetPreconfiguredMessages());

            }
        }
        private static IEnumerable<Message> GetPreconfiguredMessages()
        {
            //  var concertGuid = Guid.Parse("{253C75D5-32AF-4DBF-AB63-1AF449BDE7BD}")//Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");

            return new List<Message>()
            {
                new Message()
                {
                    Id= new string("61cc69962ff50bdbbb0ef1f7"),
               // Id = ObjectId.Parse("61cc69962ff50bdbbb0ef1f7"),
            //Guid.Parse("{253C75D5-32AF-4DBF-AB63-1AF449BDE7BD}"),
            Code = "1",
                MessageContent = "{PropertyName} is required.",
                Language = "en",
                Type = Message.MessageType.Error
                } ,
            new Message()
            {
                 Id= new string("61cc6987cd972a2af94a8e77"),
                // Id = ObjectId.Parse("61cc6987cd972a2af94a8e77"),
            //Guid.Parse("{ED0CC6B6-11F4-4512-A441-625941917502}"),
            Code = "2",
                MessageContent = "{PropertyName} must not exceed {MaxLength} characters.",
                Language = "en",
                Type = Message.MessageType.Error

            },
            new Message()
            { Id= new string("61cc697b02083af977eaff3f"),
              //  Id =ObjectId.Parse("61cc697b02083af977eaff3f"),
            //Guid.Parse("{FAFE649A-3E2A-4153-8FD8-9DCD0B87E6D8}"),
            Code = "3",
                MessageContent = "An event with the same name and date already exists.",
                Language = "en",
                Type = Message.MessageType.Error
            }
            };
        }

        public async Task<Message> GetMessage(string Code, string Lang)
        {
            _logger.LogInformation("GetAllNotifications Initiated");
            if (!_cacheService.TryGet(cacheKey, out IReadOnlyList<Message> cachedList))
            {
                var cachedList1 = await _dbContext.FindAsync(Builders<Message>.Filter.Empty);
                // _dbContext.Set<Message>().ToListAsync();
                cachedList = await cachedList1.ToListAsync();
                _cacheService.Set(cacheKey, cachedList);
            }
            _logger.LogInformation("GetAllNotifications Completed");
            return cachedList.FirstOrDefault(x => x.Code == Code && x.Language == Lang);
        }
        /*public async Task<Message> GetMessage(string Code, string Lang)
        {
            _logger.LogInformation("GetAllNotifications Initiated");
            if (!_cacheService.TryGet(cacheKey, out IReadOnlyList<Message> cachedList))
            {
                cachedList = await _dbContext.Set<Message>().ToListAsync();
                _cacheService.Set(cacheKey, cachedList);
            }
            _logger.LogInformation("GetAllNotifications Completed");
            return cachedList.FirstOrDefault(x => x.Code == Code && x.Language == Lang);
        }*/
    }
}
