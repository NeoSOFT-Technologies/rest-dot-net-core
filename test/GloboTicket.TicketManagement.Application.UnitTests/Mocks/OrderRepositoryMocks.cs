using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GloboTicket.TicketManagement.Application.UnitTests.Mocks
{
    public class OrderRepositoryMocks
    {
        public static Mock<IOrderRepository> GetOrderRepositoryAsync()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Id = new string("61d69f9eeba87eb6b232ccca"),
                    OrderTotal = 400,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = new string("61d69fa8269cc27003d6ef4c")
                },
                new Order
                {
                    Id = new string("61d69fb422b40326d8aaa00f"),
                    OrderTotal = 135,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = new string("61d69fbdecfde870227a0211")
                },
                new Order
                {
                    Id = new string("61d69fc8a0030dde32b10ea8"),
                    OrderTotal = 85,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = new string("61d69fd0d832d1d6ed67acd8")
                },
                new Order
                {
                    Id = new string("61d69fdfc0b6fea43acd0e29"),
                    OrderTotal = 245,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = new string("61d69feb429616f01390d772")
                },
                new Order
                {
                    Id = new string("61d69ff78ebc2b8939c4f4d0"),
                    OrderTotal = 142,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = new string("61d6a00490391af5ff18d6a7")
                },
                new Order
                {
                    Id = new string("61d6a00e325d74b3515046f4"),
                    OrderTotal = 40,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = new string("61d6a01710f9647b427c793e")
                },
                new Order
                {
                    Id =new string("61d6a0225f0f5144a393acdf"),
                    OrderTotal = 116,
                    OrderPaid = true,
                    OrderPlaced = Convert.ToDateTime("2021-08-26 10:44:09.5406918"),
                    UserId = new string("61d6a02b13fd010930249ac9")
                }
            };

            var mockOrderRepository = new Mock<IOrderRepository>();

            mockOrderRepository.Setup(repo => repo.GetPagedOrdersForMonth(Convert.ToDateTime("2021-08-26 10:44:09.5406918"), 1, 2)).ReturnsAsync(
                orders.Where(x => x.OrderPlaced.Month == Convert.ToDateTime("2021-08-26 10:44:09.5406918").Month && x.OrderPlaced.Year == Convert.ToDateTime("2021-08-26 10:44:09.5406918").Year)
                .Skip((1 - 1) * 2).Take(2).ToList());
            mockOrderRepository.Setup(repo => repo.GetTotalCountOfOrdersForMonth(Convert.ToDateTime("2021-08-26 10:44:09.5406918"))).ReturnsAsync(
                orders.Count(x => x.OrderPlaced.Month == Convert.ToDateTime("2021-08-26 10:44:09.5406918").Month && x.OrderPlaced.Year == Convert.ToDateTime("2021-08-26 10:44:09.5406918").Year));

            return mockOrderRepository;
        }
    }
}
