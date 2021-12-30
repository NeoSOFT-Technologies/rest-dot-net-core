using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Application.Responses;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class GetEventDetailQueryHandler : IRequestHandler<GetEventDetailQuery, Response<EventDetailVm>>
    {
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEventDetailQueryHandler> _logger;

        private readonly IDataProtector _protector;
        public GetEventDetailQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository, IAsyncRepository<Category> categoryRepository, IDataProtectionProvider provider, ILogger<GetEventDetailQueryHandler> logger)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _categoryRepository = categoryRepository;
            _protector = provider.CreateProtector("");
            _logger = logger;
        }

        public async Task<Response<EventDetailVm>> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetEventsListQueryHandler initiated");
            string id = _protector.Unprotect(request.Id);

            var @event = await _eventRepository.GetByIdAsync(new Guid(id));
            var eventDetailDto = _mapper.Map<EventDetailVm>(@event);

            var category = await _categoryRepository.GetByIdAsync(@event.CategoryId);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), @event.CategoryId);
            }
            eventDetailDto.Category = _mapper.Map<CategoryDto>(category);

            var response = new Response<EventDetailVm>(eventDetailDto);
            _logger.LogInformation("GetEventsListQueryHandler completed");
            return response;
        }
    }
}
