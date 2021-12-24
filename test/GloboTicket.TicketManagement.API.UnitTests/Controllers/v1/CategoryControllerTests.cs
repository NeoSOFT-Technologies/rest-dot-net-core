using GloboTicket.TicketManagement.Api.Controllers.v1;
using GloboTicket.TicketManagement.API.UnitTests.Mocks;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.StoredProcedure;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using GloboTicket.TicketManagement.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.API.UnitTests.Controllers.v1
{
    public class CategoryControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<CategoryController>> _mockLogger;
        public CategoryControllerTests()
        {
            _mockLogger = new Mock<ILogger<CategoryController>>();
            _mockMediator = MediatorMocks.GetMediator();
        }

        [Fact]
        public async Task Get_CategoryList()
        {
            var controller = new CategoryController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.GetAllCategories();

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<IEnumerable<CategoryListVm>>>();
        }

        [Fact]
        public async Task Get_Categories_WithEvents()
        {
            var controller = new CategoryController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.GetCategoriesWithEvents(true);

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<IEnumerable<CategoryEventListVm>>>();
        }

        [Fact]
        public async Task Post_Category()
        {
            var controller = new CategoryController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.Create(new CreateCategoryCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<CreateCategoryDto>>();
        }

        [Fact]
        public async Task StoredProcedure_Demo()
        {
            var controller = new CategoryController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.StoredProcedureDemo(new StoredProcedureCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<StoredProcedureDto>>();
        }
    }
}
