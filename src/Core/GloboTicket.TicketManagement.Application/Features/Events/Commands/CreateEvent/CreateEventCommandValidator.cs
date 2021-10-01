using FluentValidation;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        public CreateEventCommandValidator(IEventRepository eventRepository, IReadOnlyList<Notification> messages)
        {
            _eventRepository = eventRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "1").NotificationMessage)
                .NotNull()
                .MaximumLength(50).WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "2").NotificationMessage);

            RuleFor(p => p.Date)
                .NotEmpty().WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "1").NotificationMessage)
                .NotNull()
                .GreaterThan(DateTime.Now);

            RuleFor(e => e)
                .MustAsync(EventNameAndDateUnique)
                .WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "3").NotificationMessage);

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "1").NotificationMessage)
                .GreaterThan(0);
        }

        private async Task<bool> EventNameAndDateUnique(CreateEventCommand e, CancellationToken token)
        {
            return !(await _eventRepository.IsEventNameAndDateUnique(e.Name, e.Date));
        }
    }
}
