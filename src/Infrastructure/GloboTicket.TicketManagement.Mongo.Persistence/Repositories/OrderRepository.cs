using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
/*using Microsoft.EntityFrameworkCore;*/
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Mongo.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ILogger _logger;
        public OrderRepository(/*GloboTicketDbContext*/ /*IMongoDbContext */IMongoDbSettings dbContext, ILogger<Order> logger) : base(dbContext, logger)
        {
            _logger = logger;
            /*ContextSeedData<Order>.*/SeedData(_dbContext);
        }
        public static void SeedData(IMongoCollection<Order> dataCollection)
        {
            //check if there is any existing product collections
            bool existProduct = dataCollection.Find(p => true).Any();
            if (!existProduct)
            {
                dataCollection.InsertManyAsync(GetPreconfiguredOrder());

            }
        }
        private static IEnumerable<Order> GetPreconfiguredOrder()
        {
            var concertGuid = ObjectId.Parse("61cc69962ff50bdbbb0ef1f7");
                //Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");

            return new List<Order>()
            {
                new Order()
                {
                 Id =ObjectId.Parse("61cc69962ff50bdbbb0ef1f7"),
                 //Guid.Parse("{7E94BC5B-71A5-4C8C-BC3B-71BB7976237E}"),
                OrderTotal = 400,
                OrderPaid = true,
                OrderPlaced = DateTime.Now,
                UserId =ObjectId.Parse("61cc6a3952c4e504b0742bba")
                //Guid.Parse("{A441EB40-9636-4EE6-BE49-A66C5EC1330B}")
                }
            };
        }

        public async Task<List<Order>> GetPagedOrdersForMonth(DateTime date, int page, int size)
        {
            _logger.LogInformation("GetPagedOrdersForMonth Initiated");
/*
            FilterDefinition<Order> filterData = Builders<Order>.Filter.Eq(p => p.OrderPlaced.Month, date.Month) & Builders<Order>.Filter.Eq(p => p.OrderPlaced.Year, date.Year);*/
            return await _dbContext.AsQueryable<Order>().Where(p=>p.OrderPlaced.Date==date/*p => p.OrderPlaced.Month == date.Month  && p.OrderPlaced.Year == date.Year*/).Skip((page - 1) * size).Take(size).ToListAsync();
            //filter records
            /*return await _dbContext.Orders.Where(x => x.OrderPlaced.Month == date.Month && x.OrderPlaced.Year == date.Year)
                .Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();*/
        }

        public async Task<int> GetTotalCountOfOrdersForMonth(DateTime date)
        {
            _logger.LogInformation("GetPagedOrdersForMonth Initiated");
            /* FilterDefinition<Order> filterData = Builders<Order>.Filter.Eq(p => p.OrderPlaced.Month, date.Month) & Builders<Order>.Filter.Eq(p => p.OrderPlaced.Year, date.Year);
             return (int)await _dbContext.CountAsync(filterData, null);*/
            return await _dbContext.AsQueryable<Order>().CountAsync(p => p.OrderPlaced.Month == date.Month && p.OrderPlaced.Year == date.Year);
            //  return await _dbContext.Orders.CountAsync(x => x.OrderPlaced.Month == date.Month && x.OrderPlaced.Year == date.Year);
        }





       /* public async Task<List<Order>> GetPagedOrdersForMonth(DateTime date, int page, int size)
        {
            _logger.LogInformation("GetPagedOrdersForMonth Initiated");

            return await _dbContext.Orders.Where(x => x.OrderPlaced.Month == date.Month && x.OrderPlaced.Year == date.Year)
         .Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async Task<int> GetTotalCountOfOrdersForMonth(DateTime date)
        {
            _logger.LogInformation("GetPagedOrdersForMonth Initiated");
            return await _dbContext.Orders.CountAsync(x => x.OrderPlaced.Month == date.Month && x.OrderPlaced.Year == date.Year);
        }*/
    }
}
