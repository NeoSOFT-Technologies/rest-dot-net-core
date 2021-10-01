using FluentValidation;
using GloboTicket.TicketManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator(IReadOnlyList<Notification> messages)
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "1").NotificationMessage)
                .NotNull()
                .MaximumLength(50).WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "2").NotificationMessage);

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "1").NotificationMessage)
                .GreaterThan(0);
        }
    }
}
