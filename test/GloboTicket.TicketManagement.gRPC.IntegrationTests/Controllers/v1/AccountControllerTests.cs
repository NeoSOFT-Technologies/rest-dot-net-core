using GloboTicket.TicketManagement.gRPC.IntegrationTests.Base;
using GrpcAccountClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace GloboTicket.TicketManagement.gRPC.IntegrationTests.Controllers.v1
{
    [Collection("Database")]
    public class AccountControllerTests:IClassFixture<CustomWebApplicationFactory>
    {
        private readonly AccountProtoService.AccountProtoServiceClient _client;

        public AccountControllerTests(CustomWebApplicationFactory factory)
        {
            _client =new AccountProtoService.AccountProtoServiceClient(factory.channel);
        }


        [Fact]
        public async Task Post_Register_ReturnsSuccessResult()
        {
            var request = new RegisterReq()
            {
                FirstName = "Fname",
                LastName = "Lname",
                Email = "fname@test.com",
                UserName = "fname.lname",
                Password = "User123!@#"
            };

            var response = _client.RegisterAsync(request);
            response.ShouldNotBeNull();
        }

        [Fact]
        public async Task Post_Authenticate_ReturnsSuccessResult()
        {
            var request = new AuthenticateReq()
            {
                Email = "john@test.com",
                Password = "User123!@#"
            };

            var response = await _client.AuthenticateAsync(request);

            response.IsAuthenticated.ShouldBeEquivalentTo(true);
            response.Token.ShouldNotBeNull();
            response.RefreshToken.ShouldNotBeNull();
        }
    }
}
