using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Infrastructure.Mail;
using GloboTicket.TicketManagement.Infrastructure.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Infrastructure.UnitTests.Mail
{
    public class EmailServiceTests
    {
        public IOptions<EmailSettings> _emailSettings { get; }
        public Mock<ILogger<EmailService>> _logger { get; }
        public Mock<ISendGridClient> _mockSendGridClient;
        public EmailServiceTests()
        {
            _emailSettings = Options.Create(new EmailSettings()
            {
                FromAddress = "sender@test.com",
                ApiKey = "TestKey",
                FromName = "Sender1"
            });
            _logger = new Mock<ILogger<EmailService>>();
            _mockSendGridClient = SendGridClientMocks.GetSendGridClient();
        }

        [Fact]
        public async Task Email_Sent_OK()
        {
            var service = new EmailService(_emailSettings, _logger.Object, _mockSendGridClient.Object);

            var result = await service.SendEmail(new Email()
            {
                Body = "Body",
                Subject = "Subject",
                To = "receiver1@test.com"
            });

            result.ShouldBeOfType<bool>();
            result.ShouldBe(true);
        }

        [Fact]
        public async Task Email_Sent_Accepted()
        {
            var service = new EmailService(_emailSettings, _logger.Object, _mockSendGridClient.Object);

            var result = await service.SendEmail(new Email()
            {
                Body = "Body",
                Subject = "Subject",
                To = "receiver2@test.com"
            });

            result.ShouldBeOfType<bool>();
            result.ShouldBe(true);
        }

        [Fact]
        public async Task Email_Not_Sent()
        {
            var service = new EmailService(_emailSettings, _logger.Object, _mockSendGridClient.Object);

            var result = await service.SendEmail(new Email()
            {
                Body = "Body",
                Subject = "Subject",
                To = "receiver3@test.com"
            });

            result.ShouldBeOfType<bool>();
            result.ShouldBe(false);
        }
    }
}
