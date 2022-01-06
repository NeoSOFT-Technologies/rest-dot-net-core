using MediatR;
using System.Threading.Tasks;
using Grpc.Core;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsList;
using AutoMapper;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using Event.V1;

namespace GloboTicket.TicketManagement.gRPC.Services.v1
{
    public class EventService: Event.V1.EventProtoService.EventProtoServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<EventModel> GetEventById(EventRequest request, ServerCallContext context)
        {
            var getEventDetailQuery = new GetEventDetailQuery() { Id = request.Id };
            var dtos = await _mediator.Send(getEventDetailQuery);
            var EventModel = _mapper.Map<EventModel>(dtos.Data);
            return EventModel;
        }

        public override async Task<EventModelList> GetAllEvent(AllEventRequest request, ServerCallContext context)
        {
            var getEventsListQuery = new GetEventsListQuery();
            var dtos = await _mediator.Send(getEventsListQuery);
            var EventModelList = new EventModelList();
            foreach (var item in dtos.Data)
            {
                EventModelList.Eventmodel.Add(_mapper.Map<EventModel>(item));
            }
            return EventModelList;
        }

        public override async Task<EventModelReturn> CreateEvent(CreateEventRequest request, ServerCallContext context)
        {
            var CreateEvent = _mapper.Map<CreateEventCommand>(request);
            var result = await _mediator.Send(CreateEvent);
            var response = _mapper.Map<EventModelReturn>(result);
            return response;

        }
        public override async Task<EventModelReturn> UpdateEvent(UpdateEventRequest request, ServerCallContext context)
        {
            var UpdateEventRequest = _mapper.Map<UpdateEventCommand>(request);
            var result = await _mediator.Send(UpdateEventRequest);
            var response = _mapper.Map<EventModelReturn>(result);
            return response;
        }

        public override async Task<DeleteEventReturn> DeleteEvent(DeleteEventRequest request, ServerCallContext context)
        {
            var deleteEventCommand = new DeleteEventCommand() { EventId = request.EventId };
            await _mediator.Send(deleteEventCommand);
            var DeleteEventReturn = new DeleteEventReturn() { Message = "Done" };
            return DeleteEventReturn;
        }
    }
}
