using GloboTicket.TicketManagement.Application.Contracts.Identity;
using GloboTicket.TicketManagement.Application.Models.Authentication;
using Moq;

namespace GloboTicket.TicketManagement.API.UnitTests.Mocks
{
    public class AuthenticationServiceMocks
    {
        public static Mock<IAuthenticationService> GetAuthenticationService()
        {
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            mockAuthenticationService.Setup(auth => auth.AuthenticateAsync(It.IsAny<AuthenticationRequest>())).ReturnsAsync(new AuthenticationResponse());
            mockAuthenticationService.Setup(auth => auth.RegisterAsync(It.IsAny<RegistrationRequest>())).ReturnsAsync(new RegistrationResponse());
            mockAuthenticationService.Setup(auth => auth.RefreshTokenAsync(It.IsAny<RefreshTokenRequest>())).ReturnsAsync(new RefreshTokenResponse());

            return mockAuthenticationService;
        }
    }
}
