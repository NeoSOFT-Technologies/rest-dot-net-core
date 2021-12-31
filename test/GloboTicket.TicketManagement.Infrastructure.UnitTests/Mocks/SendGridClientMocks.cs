using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading;

namespace GloboTicket.TicketManagement.Infrastructure.UnitTests.Mocks
{
    public class SendGridClientMocks
    {
        public static Mock<ISendGridClient> GetSendGridClient()
        {
            var mockSendGridClient = new Mock<ISendGridClient>();

            mockSendGridClient.Setup(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SendGridMessage sendGridMessage, CancellationToken cancellationToken) => 
                {
                    var to = sendGridMessage.Personalizations[0].Tos[0].Email;
                    if (to == "receiver1@test.com") 
                        return new Response(HttpStatusCode.OK, null, null);
                    else if (to == "receiver2@test.com")
                        return new Response(HttpStatusCode.Accepted, null, null);
                    return new Response(HttpStatusCode.InternalServerError, null, null);
                });

            return mockSendGridClient;
        }
    }
}
