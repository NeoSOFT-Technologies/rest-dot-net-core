using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GloboTicket.TicketManagement.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
