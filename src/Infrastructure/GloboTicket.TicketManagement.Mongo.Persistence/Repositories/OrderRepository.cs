using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Mongo.Persistence.Settings;
using Microsoft.Extensions.Logging;
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
        public OrderRepository(IMongoDbSettings dbContext, ILogger<Order> logger) : base(dbContext, logger)
        {
            _logger = logger;
        }
       
        public async Task<List<Order>> GetPagedOrdersForMonth(DateTime date, int page, int size)
        {
            _logger.LogInformation("GetPagedOrdersForMonth Initiated");
            
            var allOrders = await _dbContext.FindAsync(Builders<Order>.Filter.Empty).Result.ToListAsync();

            return allOrders.AsQueryable().Where(p => p.OrderPlaced.Month == date.Month && p.OrderPlaced.Year == date.Year).Skip((page - 1) * size).Take(size).ToList();
        }

        public async Task<int> GetTotalCountOfOrdersForMonth(DateTime date)
        {
            _logger.LogInformation("GetPagedOrdersForMonth Initiated");
            var allOrders = await _dbContext.FindAsync(Builders<Order>.Filter.Empty).Result.ToListAsync();
            return allOrders.AsQueryable().Where(p => p.OrderPlaced.Month == date.Month && p.OrderPlaced.Year == date.Year).Count();
        }

    }
}
