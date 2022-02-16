using AutoMapper;
using Category.V1;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.gRPC.Services.v1
{
    public class CategoryService:Category.V1.CategoryProtoService.CategoryProtoServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        readonly ILogger _logger;
        public CategoryService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<ListOfCategories> GetAllCategories(GetCategoriesRequest request, ServerCallContext context)
        {
            //_logger.LogInformation("GetAllCategories Initiated");
            var dtos = await _mediator.Send(new GetCategoriesListQuery());
            //_logger.LogInformation("GetAllCategories Completed");
            ListOfCategories allCategories = new ListOfCategories();
            foreach (var item in dtos.Data)
            {
                allCategories.CategoryModel.Add(_mapper.Map<CategoriesModel>(item));
            }
            return allCategories;
        }

        public override async Task<GetCategoriesListWithEventsQueryResponse> GetCategoriesListWithEventsQuery(GetCategoriesListWithEventsQueryRequest request, ServerCallContext context)
        {
            var response = new GetCategoriesListWithEventsQueryResponse();

            GetCategoriesListWithEventsQuery getCategoriesListWithEventsQuery = new GetCategoriesListWithEventsQuery() { IncludeHistory = request.IncludeHistory };
            var dtos = await _mediator.Send(getCategoriesListWithEventsQuery);
            foreach (var item in dtos.Data)
            {
                foreach (var item3 in response.GetAllCategorieswithevents)
                {
                    foreach (var item1 in item.Events)
                    {
                        item3.CatEventdto.Add(_mapper.Map<CategoryEventDto1>(item1));
                    }
                }
                response.GetAllCategorieswithevents.Add(_mapper.Map<GetCategoriesListWithEvents>(item));

            }
            return response;
        }

        public override async Task<AddCategoryResponse> AddCategory(AddCategoryRequest request, ServerCallContext context)
        {
            var req = _mapper.Map<CreateCategoryCommand>(request);
            var response = await _mediator.Send(req);
            var resp = _mapper.Map<AddCategoryResponse>(response.Data);
            return resp;
        }
    }
}
