using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GloboTicket.TicketManagement.Application.UnitTests.Mocks
{
    public class MessageRepositoryMocks
    {
        public static Mock<IMessageRepository> GetMessageRepository()
        {
            var messages = new List<Message>
            {
                new Message
                {
                    MessageId = Guid.NewGuid(),
                    ErrorCode = "1",
                    ErrorMessage = "{PropertyName} is required.",
                    Language = "en"
                },
                new Message
                {
                    MessageId = Guid.NewGuid(),
                    ErrorCode = "2",
                    ErrorMessage = "{PropertyName} must not exceed {MaxLength} characters.",
                    Language = "en"
                },
                new Message
                {
                    MessageId = Guid.NewGuid(),
                    ErrorCode = "3",
                    ErrorMessage = "An event with the same name and date already exists.",
                    Language = "en"
                }
            };

            var mockMessageRepository = new Mock<IMessageRepository>();

            mockMessageRepository.Setup(repo => repo.GetMessage(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
                (string ErrorCode, string Lang) =>
                {
                    return messages.FirstOrDefault(x => x.ErrorCode == ErrorCode && x.Language == Lang).ErrorMessage;
                });

            return mockMessageRepository;
        }
    }
}
