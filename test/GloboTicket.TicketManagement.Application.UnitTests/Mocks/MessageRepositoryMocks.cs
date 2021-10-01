using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;

namespace GloboTicket.TicketManagement.Application.UnitTests.Mocks
{
    public class MessageRepositoryMocks
    {
        public static Mock<IMessageRepository> GetMessageRepository()
        {
            var messages = new List<Notification>
            {
                new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationCode = "1",
                    NotificationMessage = "{PropertyName} is required."
                },
                new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationCode = "2",
                    NotificationMessage = "{PropertyName} must not exceed {MaxLength} characters."
                },
                new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationCode = "3",
                    NotificationMessage = "An event with the same name and date already exists."
                }
            };

            var mockMessageRepository = new Mock<IMessageRepository>();

            mockMessageRepository.Setup(repo => repo.GetAllNotifications()).ReturnsAsync(messages);

            return mockMessageRepository;
        }
    }
}
