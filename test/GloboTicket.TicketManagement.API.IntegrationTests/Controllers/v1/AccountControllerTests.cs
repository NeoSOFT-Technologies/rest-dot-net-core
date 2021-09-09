using GloboTicket.TicketManagement.Api;
using GloboTicket.TicketManagement.API.IntegrationTests.Base;
using GloboTicket.TicketManagement.Application.Models.Authentication;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Controllers.v1
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _factory;
        public AccountControllerTests(WebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Post_Authenticate_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var request = new AuthenticationRequest()
            {
                Email = "john@test.com",
                Password = "User123!@#"
            };

            var requestJson = JsonConvert.SerializeObject(request);

            HttpContent content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api​/v1​/account​/authenticate", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<AuthenticationResponse>(responseString);

            result.IsAuthenticated.ShouldBeEquivalentTo(true);
            result.Token.ShouldNotBeNull();
            result.RefreshToken.ShouldNotBeNull();
        }

        [Fact]
        public async Task Post_Register_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var request = new RegistrationRequest()
            {
                FirstName = "Apoorv",
                LastName = "Rane",
                Email = "apoorv@test.com",
                UserName = "apoorv",
                Password = "User123!@#"
            };

            var requestJson = JsonConvert.SerializeObject(request);

            HttpContent content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api​/v1​/account​/register", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<RegistrationResponse>(responseString);

            result.UserId.ShouldNotBeNull();
        }

        [Fact]
        public async Task Post_RefreshToken_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var request = new RefreshTokenRequest()
            {
                Token = "m5U+6YUCnxCinVmqMTGPoY1X1xUIPHe/P0HF5OR6KlQ="
            };

            var requestJson = JsonConvert.SerializeObject(request);

            HttpContent content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api​/v1​/account​/refresh-token", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<RefreshTokenResponse>(responseString);

            result.IsAuthenticated.ShouldBeEquivalentTo(true);
            result.Token.ShouldNotBeNull();
            result.RefreshToken.ShouldNotBeNull();
        }

        [Fact]
        public async Task Post_RevokeToken_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var request = new RevokeTokenRequest()
            {
                Token = "m5U+6YUCnxCinVmqMTGPoY1X1xUIPHe/P0HF5OR6KlQ="
            };

            var requestJson = JsonConvert.SerializeObject(request);

            HttpContent content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api​/v1​/account​/revoke-token", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<RevokeTokenResponse>(responseString);

            result.IsRevoked.ShouldBeEquivalentTo(true);
        }
    }
}
