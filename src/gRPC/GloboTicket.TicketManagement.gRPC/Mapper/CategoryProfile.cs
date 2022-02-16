using AutoMapper;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry;
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
            CreateMap<CreateCategoryCommand, Category.V2.AddCategoryRequest>().ReverseMap();
            CreateMap<CreateCategoryDto, Category.V2.AddCategoryResponse>().ReverseMap();
            CreateMap<Category.V1.CategoriesModel, CategoryListVm>().ReverseMap();
            CreateMap<Category.V2.CategoriesModel, CategoryListVm>().ReverseMap();
            CreateMap<CategoryEventListVm, Category.V1.GetCategoriesListWithEvents>().ReverseMap();
            CreateMap<CategoryEventListVm, Category.V2.GetCategoriesListWithEvents>().ReverseMap();
            CreateMap<CategoryEventDto, Category.V1.CategoryEventDto1>().ReverseMap();
            CreateMap<CategoryEventDto, Category.V2.CategoryEventDto1>().ReverseMap();
            CreateMap<Response<IEnumerable<CategoryEventListVm>>, Category.V1.GetCategoriesListWithEventsQueryResponse>().ReverseMap();
        }
    }
}
