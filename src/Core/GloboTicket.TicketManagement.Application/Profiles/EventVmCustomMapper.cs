using AutoMapper;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsList;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.AspNetCore.DataProtection;

namespace GloboTicket.TicketManagement.Application.Profiles
{
    public class EventVmCustomMapper : ITypeConverter<Event, EventListVm>
    {
        public EventListVm Convert(Event source, EventListVm destination, ResolutionContext context)
        {
            var dataProtectionProvider = DataProtectionProvider.Create("Test");
            var protector = dataProtectionProvider.CreateProtector("Test");
            EventListVm dest = new EventListVm()
            {
                EventId = protector.Protect(source.EventId.ToString()),
                Name = source.Name,
                ImageUrl = source.ImageUrl,
                Date = source.Date
            };

            return dest;

        }
    }
}
