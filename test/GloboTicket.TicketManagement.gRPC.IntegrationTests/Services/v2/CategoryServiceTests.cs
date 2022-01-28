using GloboTicket.TicketManagement.gRPC.IntegrationTests.Base;
using Grpc.Net.Client;
using GrpcAccountClient;
using GrpcCategoryClient.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace GloboTicket.TicketManagement.gRPC.IntegrationTests.Services.v2
{
    [Collection("Database")]
    public class CategoryServiceTests:IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CategoryProtoService.CategoryProtoServiceClient _client;
        private readonly AccountProtoService.AccountProtoServiceClient _accountClient;

        public CategoryServiceTests(CustomWebApplicationFactory factory)
        {
            _client = new CategoryProtoService.CategoryProtoServiceClient(factory.channel);
            _accountClient = new AccountProtoService.AccountProtoServiceClient(factory.channel);
        }

        //[Fact]
        //public async Task Get_CategoriesList_ReturnsUnauthorizedResult()
        //{

        //    var response = await _client.GetAllCategoriesAsync(new GetCategoriesRequest());

        //    response.ShouldNotBeNull();
        //}

        [Fact]
        public async Task Get_CategoriesListWithEvents_IncludeHistory_ReturnsSuccessResult()
        {
            var AdminUser = await GetAuthenicatedUserAsync("Admin");
            var headers = new Grpc.Core.Metadata
                {
                    { "Authorization", $"Bearer {AdminUser.Token}" }
                };

            var response = await _client.GetCategoriesListWithEventsQueryAsync(new GetCategoriesListWithEventsQueryRequest(), headers);
            response.ShouldNotBeNull();
            response.ShouldBeOfType<GetCategoriesListWithEventsQueryResponse>();
        }

        //[Fact]
        //public async Task Get_CategoriesListWithEvents_IncludeHistory_ReturnsUnauthorizedResult()
        //{

        //    var response = await _client.GetCategoriesListWithEventsQueryAsync(new GetCategoriesListWithEventsQueryRequest());

        //}

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



        private async Task<AuthenticateResp> GetAuthenicatedUserAsync(string role)
        {
            var request = new AuthenticateReq();
            if (role == "Admin")
            {
                request.Email = "john@test.com";
                request.Password = "User123!@#";
            }
            else
            {
                request.Email = "shivamgupta@gmail.com";
                request.Password = "User123!@#";
            }

            var response =await _accountClient.AuthenticateAsync(request);
            return response;
        }
    }
}
