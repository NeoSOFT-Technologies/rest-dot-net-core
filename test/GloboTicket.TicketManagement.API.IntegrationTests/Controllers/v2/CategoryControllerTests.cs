using GloboTicket.TicketManagement.API.IntegrationTests.Base;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using GloboTicket.TicketManagement.Application.Models.Authentication;
using GloboTicket.TicketManagement.Application.Responses;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Controllers.v2
{
    [Collection("Database")]
    public class CategoryControllerTests : IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _factory;
        public CategoryControllerTests(WebApplicationFactory factory)
        {
            _factory = factory;
        }

       

        [Fact]
        public async Task Get_CategoriesList_ReturnsUnauthorizedResult()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/v2/category/all");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        

        [Fact]
        public async Task Get_CategoriesListWithEvents_IncludeHistory_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var authenticationResponse = await AuthenticateAsync("Administrator");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResponse.Token);

            var response = await client.GetAsync("/api/v2/Category/allwithevents?includeHistory=true");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<IEnumerable<CategoryEventListVm>>>(responseString);

            result.Data.ShouldBeOfType<List<CategoryEventListVm>>();
            result.Data.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task Get_CategoriesListWithEvents_IncludeHistory_ReturnsUnauthorizedResult()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/v2/Category/allwithevents?includeHistory=true");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_CategoriesListWithEvents_DoNotIncludeHistory_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var authenticationResponse = await AuthenticateAsync("Administrator");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResponse.Token);
            
            var response = await client.GetAsync("/api/v2/Category/allwithevents?includeHistory=false");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<IEnumerable<CategoryEventListVm>>>(responseString);

            result.Data.ShouldBeOfType<List<CategoryEventListVm>>();
            result.Data.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task Post_Category_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var category = new CreateCategoryCommand()
            {
                Name = "Test"
            };

            var categoryJson = JsonConvert.SerializeObject(category);

            HttpContent content = new StringContent(categoryJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/v2/category", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<CreateCategoryDto>>(responseString);

            result.Succeeded.ShouldBeEquivalentTo(true);
            result.Data.ShouldBeOfType<CreateCategoryDto>();
            result.Errors.ShouldBeNull();
        }

        private async Task<AuthenticationResponse> AuthenticateAsync(string role)
        {
            var client = _factory.CreateClient();

            AuthenticationRequest request = new AuthenticationRequest();
            if (role == "Administrator")
            {
                request.Email = "john@test.com";
                request.Password = "User123!@#";
            }
            else
            {
                request.Email = "apoorv@test.com";
                request.Password = "User123!@#";
            }
            
            var requestJson = JsonConvert.SerializeObject(request);

            HttpContent content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/v1/Account/authenticate", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<AuthenticationResponse>(responseString);
            return result;
        }
    }
}
