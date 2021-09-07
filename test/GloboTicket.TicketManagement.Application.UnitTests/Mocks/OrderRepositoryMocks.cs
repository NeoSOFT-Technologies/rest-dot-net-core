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
