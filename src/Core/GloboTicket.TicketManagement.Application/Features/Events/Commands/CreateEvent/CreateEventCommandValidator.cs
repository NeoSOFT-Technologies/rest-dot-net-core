using FluentValidation;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMessageRepository _messageRepository;
        public CreateEventCommandValidator(IEventRepository eventRepository, IMessageRepository messageRepository)
        {
            _eventRepository = eventRepository;
            _messageRepository = messageRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage(GetMessage("1", ApplicationConstants.LANG_ENG))
                .NotNull()
                .MaximumLength(50).WithMessage(GetMessage("2", ApplicationConstants.LANG_ENG));

            RuleFor(p => p.Date)
                .NotEmpty().WithMessage(GetMessage("1", ApplicationConstants.LANG_ENG))
                .NotNull()
                .GreaterThan(DateTime.Now);

            RuleFor(e => e)
                .MustAsync(EventNameAndDateUnique)
                .WithMessage(GetMessage("3", ApplicationConstants.LANG_ENG));

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage(GetMessage("1", ApplicationConstants.LANG_ENG))
                .GreaterThan(0);
        }

        private async Task<bool> EventNameAndDateUnique(CreateEventCommand e, CancellationToken token)
        {
            return !(await _eventRepository.IsEventNameAndDateUnique(e.Name, e.Date));
        }

        private string GetMessage(string Code, string Lang)
        {
            return _messageRepository.GetMessage(Code, Lang).Result.MessageContent.ToString();
        }
    }
}
