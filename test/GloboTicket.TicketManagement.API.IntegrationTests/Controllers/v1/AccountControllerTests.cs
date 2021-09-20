using GloboTicket.TicketManagement.API.IntegrationTests.Base;
using GloboTicket.TicketManagement.Application.Models.Authentication;
using Newtonsoft.Json;
using Shouldly;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Controllers.v1
{
    [Collection("Database")]
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

            var response = await client.PostAsync("/api/v1/Account/authenticate", content);

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
                FirstName = "Fname",
                LastName = "Lname",
                Email = "fname@test.com",
                UserName = "fname.lname",
                Password = "User123!@#"
            };

            var requestJson = JsonConvert.SerializeObject(request);

            HttpContent content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/v1/Account/register", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<RegistrationResponse>(responseString);

            result.UserId.ShouldNotBeNull();
        }

       
    }
}
