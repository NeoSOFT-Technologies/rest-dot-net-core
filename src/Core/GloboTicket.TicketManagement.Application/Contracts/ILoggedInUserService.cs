using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GloboTicket.TicketManagement.Application.Contracts
{
    public interface ILoggedInUserService
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; }
    }
}
