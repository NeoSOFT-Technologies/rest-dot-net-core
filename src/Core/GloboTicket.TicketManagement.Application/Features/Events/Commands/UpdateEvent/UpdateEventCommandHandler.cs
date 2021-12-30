using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Application.Responses;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using MongoDB.Bson;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand,Response<string/*ObjectId*//*Guid*/>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;

        public UpdateEventCommandHandler(IMapper mapper, IEventRepository eventRepository, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _messageRepository = messageRepository;
        }

        public async Task<Response<string/*ObjectId*//*Guid*/>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _eventRepository.GetByIdAsync(new ObjectId(request./*Event*/Id));

            if (eventToUpdate == null)
            {
                throw new NotFoundException(nameof(Event), request./*Event*/Id);
            }

            var validator = new UpdateEventCommandValidator(_messageRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
                throw new ValidationException(validationResult);

            _mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));

             await _eventRepository.UpdateAsync(eventToUpdate);

            return new Response<string/*ObjectId*//*Guid*/>(request./*Event*/Id.ToString(), "Updated successfully ");
          
        }
    }
}