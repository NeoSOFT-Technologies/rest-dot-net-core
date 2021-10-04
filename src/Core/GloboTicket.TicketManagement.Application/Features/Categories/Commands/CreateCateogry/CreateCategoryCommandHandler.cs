using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Responses;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Response<CreateCategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMessageRepository _messageRepository;

        public CreateCategoryCommandHandler(IMapper mapper, ICategoryRepository categoryRepository, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _messageRepository = messageRepository;
        }

        public async Task<Response<CreateCategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var createCategoryCommandResponse = new Response<CreateCategoryDto>();

            var validator = new CreateCategoryCommandValidator(_messageRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                createCategoryCommandResponse.Succeeded = false;
                createCategoryCommandResponse.Errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createCategoryCommandResponse.Errors.Add(error.ErrorMessage);
                }
            }
            else
            {
                var category = new Category() { Name = request.Name };
                category = await _categoryRepository.AddAsync(category);
                createCategoryCommandResponse.Data = _mapper.Map<CreateCategoryDto>(category);
                createCategoryCommandResponse.Succeeded = true;
                createCategoryCommandResponse.Message = "success";
            }

            return createCategoryCommandResponse;
        }
    }
}
