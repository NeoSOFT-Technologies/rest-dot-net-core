using GloboTicket.TicketManagement.Application.Responses;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry
{
    public class CreateCategoryCommandResponse
    {
        public CreateCategoryCommandResponse()
        {

        }

        public CreateCategoryDto Category { get; set; }
    }
}