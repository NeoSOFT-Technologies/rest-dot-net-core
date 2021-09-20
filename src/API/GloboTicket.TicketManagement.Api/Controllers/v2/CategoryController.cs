using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Api.Controllers.v2
{

    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public CategoryController(IMediator mediator, ILogger<CategoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("all", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllCategories()
        {
            _logger.LogInformation("GetAllCategories Initiated");
            var dtos = await _mediator.Send(new GetCategoriesListQuery());
            _logger.LogInformation("GetAllCategories Completed");
            return Ok(dtos);
        }

        [Authorize]
        [HttpGet("allwithevents", Name = "GetCategoriesWithEvents")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCategoriesWithEvents(bool includeHistory)
        {
            GetCategoriesListWithEventsQuery getCategoriesListWithEventsQuery = new GetCategoriesListWithEventsQuery() { IncludeHistory = includeHistory };

            var dtos = await _mediator.Send(getCategoriesListWithEventsQuery);
            return Ok(dtos);
        }

        [HttpPost(Name = "AddCategory")]
        public async Task<ActionResult> Create([FromBody] CreateCategoryCommand createCategoryCommand)
        {
            var response = await _mediator.Send(createCategoryCommand);
            return Ok(response);
        }
    }
}
