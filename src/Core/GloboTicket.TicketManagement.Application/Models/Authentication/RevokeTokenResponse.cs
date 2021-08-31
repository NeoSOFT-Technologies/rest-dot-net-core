namespace GloboTicket.TicketManagement.Application.Models.Authentication
{
    public class RevokeTokenResponse
    {
        public bool IsRevoked { get; set; }
        public string Message { get; set; }
    }
}
