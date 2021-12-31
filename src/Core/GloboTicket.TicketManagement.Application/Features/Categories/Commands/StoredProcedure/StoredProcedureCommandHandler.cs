using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Responses;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.StoredProcedure
{
    public class StoredProcedureCommandHandler : IRequestHandler<StoredProcedureCommand, Response<StoredProcedureDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMessageRepository _messageRepository;

        public StoredProcedureCommandHandler(IMapper mapper, ICategoryRepository categoryRepository, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _messageRepository = messageRepository;
        }

        public async Task<Response<StoredProcedureDto>> Handle(StoredProcedureCommand request, CancellationToken cancellationToken)
        {
            Response<StoredProcedureDto> createCategoryCommandResponse = null;

            var validator = new StoredProcedureCommandValidator(_messageRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                createCategoryCommandResponse = new Response<StoredProcedureDto>("failure");
                createCategoryCommandResponse.Errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createCategoryCommandResponse.Errors.Add(error.ErrorMessage);
                }
            }
            else
            {
                var category = new Category() { Name = request.Name };
                category = await _categoryRepository.AddCategory(category);
                createCategoryCommandResponse = new Response<StoredProcedureDto>(_mapper.Map<StoredProcedureDto>(category), "success");
            }

            return createCategoryCommandResponse;
        }
    }
}
