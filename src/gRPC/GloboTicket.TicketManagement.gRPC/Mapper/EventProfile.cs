using AutoMapper;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsList;
using GloboTicket.TicketManagement.Application.Responses;
using System;

namespace GloboTicket.TicketManagement.gRPC.Mapper
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventDetailVm, Event.V1.EventModel>().ReverseMap();
            CreateMap<EventListVm, Event.V1.EventModel>().ReverseMap();
            CreateMap<Response<Guid>, Event.V1.EventModelReturn>().ReverseMap();
            CreateMap<CreateEventCommand, Event.V1.CreateEventRequest>().ReverseMap();
            CreateMap<UpdateEventCommand, Event.V1.UpdateEventRequest>().ReverseMap();
        }
    }
}
