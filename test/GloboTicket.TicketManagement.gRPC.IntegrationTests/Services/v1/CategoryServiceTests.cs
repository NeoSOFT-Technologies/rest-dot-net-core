using GloboTicket.TicketManagement.gRPC.IntegrationTests.Base;
using Grpc.Net.Client;
using GrpcCategoryClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
//using static Category.V2.CategoryProtoService;

namespace GloboTicket.TicketManagement.gRPC.IntegrationTests.Services.v1
{
    [Collection("Database")]
    public class CategoryServiceTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CategoryProtoService.CategoryProtoServiceClient _client;
        public CategoryServiceTests(CustomWebApplicationFactory factory)
        {
            _client = new CategoryProtoService.CategoryProtoServiceClient(factory.channel);
        }

        [Fact]
        public async Task Get_CategoriesList_ReturnsSuccessResult()
        {
            var response = await _client.GetAllCategoriesAsync(new GetCategoriesRequest());
            response.CategoryModel.ShouldNotBeEmpty();
            response.CategoryModel.Count.ShouldNotBe(0);
            response.ShouldBeOfType<ListOfCategories>();
        }

        [Fact]
        public async Task Get_CategoriesListWithEvents_IncludeHistory_ReturnsSuccessResult()
        {
            var response = await _client.GetCategoriesListWithEventsQueryAsync(new GetCategoriesListWithEventsQueryRequest());
            response.GetAllCategorieswithevents.ShouldNotBeEmpty();
            response.GetAllCategorieswithevents.Count.ShouldNotBe(0);
            response.ShouldNotBeNull();
            response.ShouldBeOfType<GetCategoriesListWithEventsQueryResponse>();
        }

        [Fact]
        public async Task Get_CategoriesListWithEvents_ReturnsSuccessResult()
        {
            var response = await _client.GetCategoriesListWithEventsQueryAsync(new GetCategoriesListWithEventsQueryRequest());
            response.ShouldNotBeNull();
            response.ShouldBeOfType<GetCategoriesListWithEventsQueryResponse>();
        }

        [Fact]
        public async Task Post_Category_ReturnsSuccessResult()
        {
            var category = new AddCategoryRequest()
            {
                Name = "Test"

            };
            var response = await _client.AddCategoryAsync(category);
            response.CategoryId.ShouldNotBeNull();
            response.Name.ShouldNotBeNull();
            response.ShouldBeOfType<AddCategoryResponse>();
        }
    } 

}
