using FluentValidation;
using GloboTicket.TicketManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry
{
    public class CreateCategoryCommandValidator: AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator(IReadOnlyList<Notification> messages)
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "1").NotificationMessage)
                .NotNull()
                .MaximumLength(10).WithMessage(messages.FirstOrDefault(x => x.NotificationCode == "2").NotificationMessage);
        }
    }
}
