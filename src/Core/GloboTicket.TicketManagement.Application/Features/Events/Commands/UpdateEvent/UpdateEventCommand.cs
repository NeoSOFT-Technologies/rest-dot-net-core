using GloboTicket.TicketManagement.Application.Responses;
using MediatR;
using MongoDB.Bson;
using System;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommand: IRequest<Response<string/*ObjectId*//*Guid*/>>
    {
        public string/*ObjectId*/ /*Guid*/ /*Event*/Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Artist { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public /*Guid*/ string/*ObjectId*/ CategoryId { get; set; }



    }
}
