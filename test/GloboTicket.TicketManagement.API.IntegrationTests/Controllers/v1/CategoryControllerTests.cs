using GloboTicket.TicketManagement.API.IntegrationTests.Base;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;
using GloboTicket.TicketManagement.Application.Responses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using System.Net.Http;
using System.Text;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.StoredProcedure;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Controllers.v1
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
        public async Task Get_CategoriesList_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/V1/category/all");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response<IEnumerable<CategoryListVm>>>(responseString);
            result.Data.ShouldBeOfType<List<CategoryListVm>>();
            result.Data.ShouldNotBeEmpty();

        }

        [Fact]
        public async Task Get_CategoriesListWithEvents_IncludeHistory_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/v1/Category/allwithevents?includeHistory=true");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<IEnumerable<CategoryListVm>>>(responseString);

            result.Data.ShouldBeOfType<List<CategoryListVm>>();
            result.Data.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task Get_CategoriesListWithEvents_DoNotIncludeHistory_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/v1/Category/allwithevents?includeHistory=false");

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

            var response = await client.PostAsync("/api/v1/category", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<CreateCategoryDto>>(responseString);

            result.Succeeded.ShouldBeEquivalentTo(true);
            result.Data.ShouldBeOfType<CreateCategoryDto>();
            result.Errors.ShouldBeNull();
        }

        [Fact]
        public async Task Post_Category_StoredProcedure_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            var category = new StoredProcedureCommand()
            {
                Name = "Test"
            };

            var categoryJson = JsonConvert.SerializeObject(category);

            HttpContent content = new StringContent(categoryJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/v1/category/storedproceduredemo", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<StoredProcedureDto>>(responseString);

            result.Succeeded.ShouldBeEquivalentTo(true);
            result.Data.ShouldBeOfType<StoredProcedureDto>();
            result.Errors.ShouldBeNull();
        }
    }
}
