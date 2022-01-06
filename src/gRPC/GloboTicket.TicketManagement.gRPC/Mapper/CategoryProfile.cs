using AutoMapper;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using GloboTicket.TicketManagement.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.gRPC.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryCommand, Category.V1.AddCategoryRequest>().ReverseMap();
            CreateMap<CreateCategoryDto, Category.V1.AddCategoryResponse>().ReverseMap();
            CreateMap<Category.V1.CategoriesModel, CategoryListVm>().ReverseMap();
            CreateMap<CategoryEventListVm, Category.V1.GetCategoriesListWithEvents>().ReverseMap();
            CreateMap<CategoryEventDto, Category.V1.CategoryEventDto1>().ReverseMap();
            CreateMap<Response<IEnumerable<CategoryEventListVm>>, Category.V1.GetCategoriesListWithEventsQueryResponse>().ReverseMap();
        }
    }
}
