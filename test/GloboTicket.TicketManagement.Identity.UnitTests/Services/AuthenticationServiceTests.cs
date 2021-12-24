using GloboTicket.TicketManagement.Application.Models.Authentication;
using GloboTicket.TicketManagement.Identity.Services;
using GloboTicket.TicketManagement.Identity.UnitTests.Fakes;
using GloboTicket.TicketManagement.Identity.UnitTests.Mocks;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Identity.UnitTests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<FakeUserManager> _mockUserManager;
        private readonly Mock<FakeSignInManager> _mockSignInManager;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public AuthenticationServiceTests()
        {
            _mockUserManager = UserManagerMocks.GetUserManager();
            _mockSignInManager = SignInManagerMocks.GetSignInManager();
            _jwtSettings = Options.Create(new JwtSettings()
            {
                Key = "84322CFB66934ECC86D547C5CF4F2EFC",
                Issuer = "GloboTicketIdentity",
                Audience = "GloboTicketIdentityUser",
                DurationInMinutes = 60
            });
        }

        [Fact]
        public async Task Authenticate_Valid_User()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var email = "test@test.it";

            var result = await service.AuthenticateAsync(new AuthenticationRequest()
            {
                Email = email
            });

            result.ShouldNotBeNull();
            result.IsAuthenticated.ShouldBe(true);
            result.Email.ToLower().ShouldBe(email.ToLower());
            result.Token.ShouldNotBeNull();
        }

        [Fact]
        public async Task Handle_User_NotFound_Authenticate()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var email = "test2@test.it";

            var result = await service.AuthenticateAsync(new AuthenticationRequest()
            {
                Email = email
            });

            result.ShouldNotBeNull();
            result.IsAuthenticated.ShouldBe(false);
            result.Message.ToLower().ShouldBe($"no accounts registered with {email.ToLower()}.");
        }

        [Fact]
        public async Task Handle_Invalid_Credentials()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var email = "test@test.it";

            var result = await Should.ThrowAsync<AuthenticationException>(() => service.AuthenticateAsync(new AuthenticationRequest()
            {
                Email = email,
                Password = "Password"
            }));

            result.ShouldNotBeNull();
            result.ShouldBeOfType<AuthenticationException>();
            result.Message.ToLower().ShouldBe($"credentials for '{email.ToLower()}' aren't valid'.");
        }

        [Fact]
        public async Task Authenticate_Valid_User_Generate_RefreshToken()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var email = "test1@test.it";
            var oldRefreshTokenCount = _mockUserManager.Object.Users.FirstOrDefault(x => x.Email == email).RefreshTokens.Count;

            var result = await service.AuthenticateAsync(new AuthenticationRequest()
            {
                Email = email
            });

            var newRefreshTokenCount = _mockUserManager.Object.Users.FirstOrDefault(x => x.Email == email).RefreshTokens.Count;

            newRefreshTokenCount.ShouldBe(oldRefreshTokenCount + 1);
            result.ShouldNotBeNull();
            result.IsAuthenticated.ShouldBe(true);
            result.Email.ToLower().ShouldBe(email.ToLower());
            result.Token.ShouldNotBeNull();
        }

        [Fact]
        public async Task Register_Valid_User()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var email = "test2@test.it";
            var userName = "Test2";
            var oldCount = _mockUserManager.Object.Users.Count();

            var result = await service.RegisterAsync(new RegistrationRequest()
            {
                UserName = userName,
                Email = email,
                Password = "User123!@#"
            });

            var newCount = _mockUserManager.Object.Users.Count();

            newCount.ShouldBe(oldCount + 1);
            result.ShouldNotBeNull();
            result.UserId.ShouldNotBeNull();
        }

        [Fact]
        public async Task Register_Existing_UserName()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var email = "test2@test.it";
            var userName = "Test1";
            var oldCount = _mockUserManager.Object.Users.Count();

            var result = await Should.ThrowAsync<ArgumentException>(() => service.RegisterAsync(new RegistrationRequest()
            {
                UserName = userName,
                Email = email,
                Password = "User123!@#"
            }));

            var newCount = _mockUserManager.Object.Users.Count();

            newCount.ShouldBe(oldCount);
            result.ShouldNotBeNull();
            result.Message.ToLower().ShouldBe($"username '{userName.ToLower()}' already exists.");
        }

        [Fact]
        public async Task Register_Existing_Email()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var email = "test1@test.it";
            var userName = "Test2";
            var oldCount = _mockUserManager.Object.Users.Count();

            var result = await Should.ThrowAsync<ArgumentException>(() => service.RegisterAsync(new RegistrationRequest()
            {
                UserName = userName,
                Email = email,
                Password = "User123!@#"
            }));

            var newCount = _mockUserManager.Object.Users.Count();

            newCount.ShouldBe(oldCount);
            result.ShouldNotBeNull();
            result.Message.ToLower().ShouldBe($"email '{email.ToLower()}' already exists.");
        }

        [Fact]
        public async Task Register_UserCreation_Failed()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var email = "test2@test.it";
            var userName = "Test2";
            var oldCount = _mockUserManager.Object.Users.Count();

            var result = await Should.ThrowAsync<Exception>(() => service.RegisterAsync(new RegistrationRequest()
            {
                UserName = userName,
                Email = email
            }));

            var newCount = _mockUserManager.Object.Users.Count();

            newCount.ShouldBe(oldCount);
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task Refresh_Token()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var token = "Token";
            var email = "test@test.it";
            var oldRefreshTokenCount = _mockUserManager.Object.Users.FirstOrDefault(x => x.Email == email).RefreshTokens.Count;

            var result = await service.RefreshTokenAsync(new RefreshTokenRequest()
            {
                Token = token
            });

            var newRefreshTokenCount = _mockUserManager.Object.Users.FirstOrDefault(x => x.Email == email).RefreshTokens.Count;

            newRefreshTokenCount.ShouldBe(oldRefreshTokenCount + 1);
            result.ShouldNotBeNull();
            result.IsAuthenticated.ShouldBe(true);
            result.Email.ToLower().ShouldBe(email.ToLower());
            result.Token.ShouldNotBeNull();
            result.RefreshToken.ShouldNotBeNull();
        }

        [Fact]
        public async Task Refresh_Token_DidNot_Match()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var token = "Token1";

            var result = await service.RefreshTokenAsync(new RefreshTokenRequest()
            {
                Token = token
            });

            result.ShouldNotBeNull();
            result.IsAuthenticated.ShouldBe(false);
            result.Message.ToLower().ShouldBe("token did not match any users.");
        }

        [Fact]
        public async Task Refresh_Token_Not_Active()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var token = "ExpiredToken";

            var result = await service.RefreshTokenAsync(new RefreshTokenRequest()
            {
                Token = token
            });

            result.ShouldNotBeNull();
            result.IsAuthenticated.ShouldBe(false);
            result.Message.ToLower().ShouldBe("token not active.");
        }

        [Fact]
        public async Task Revoke_Token()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var token = "Token";

            var result = await service.RevokeToken(new RevokeTokenRequest()
            {
                Token = token
            });

            result.ShouldNotBeNull();
            result.IsRevoked.ShouldBe(true);
            result.Message.ToLower().ShouldBe("token revoked");
        }

        [Fact]
        public async Task Revoke_Empty_Token()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var token = "";

            var result = await service.RevokeToken(new RevokeTokenRequest()
            {
                Token = token
            });

            result.ShouldNotBeNull();
            result.IsRevoked.ShouldBe(false);
            result.Message.ToLower().ShouldBe("token is required");
        }

        [Fact]
        public async Task Revoke_Token_DidNot_Match()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var token = "Token1";

            var result = await service.RevokeToken(new RevokeTokenRequest()
            {
                Token = token
            });

            result.ShouldNotBeNull();
            result.IsRevoked.ShouldBe(false);
            result.Message.ToLower().ShouldBe("token did not match any users");
        }

        [Fact]
        public async Task Revoke_Token_Not_Active()
        {
            var service = new AuthenticationService(_mockUserManager.Object, _jwtSettings, _mockSignInManager.Object);
            var token = "ExpiredToken";

            var result = await service.RevokeToken(new RevokeTokenRequest()
            {
                Token = token
            });

            result.ShouldNotBeNull();
            result.IsRevoked.ShouldBe(false);
            result.Message.ToLower().ShouldBe("token is not active");
        }
    }
}
