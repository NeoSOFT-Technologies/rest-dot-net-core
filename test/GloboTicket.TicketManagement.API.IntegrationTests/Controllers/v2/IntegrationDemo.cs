using GloboTicket.TicketManagement.Api;
using GloboTicket.TicketManagement.API.IntegrationTests.Base;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;
using GloboTicket.TicketManagement.Application.Responses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using GloboTicket.TicketManagement.Domain.Entities;
using System.Net.Http;
using System.Text;
using System;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Controllers.v2
{
 


        [Collection("Database")]
        public sealed class IntegrationDemoTests : IClassFixture<BlogWebApplicationFactory>
        {
            private readonly BlogWebApplicationFactory _factory;

            public IntegrationDemoTests(BlogWebApplicationFactory factory)
            {
                _factory = factory;
            }

            //[Fact]
            public async Task Create_ShouldCreateBlog()
            {
               
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/V1/category/all");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<IEnumerable<CategoryListVm>>>(responseString);

            result.Data.ShouldBeOfType<List<CategoryListVm>>();
            result.Data.ShouldNotBeEmpty();
        }
        }
    }
