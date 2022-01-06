/*using Microsoft.AspNetCore.Identity;*/
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;

namespace GloboTicket.TicketManagement.mongo.Identity.Models
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        // public ICollection<RefreshToken> RefreshTokens { get; set; }
        // public RefreshToken refreshToken /*{ get; set; }*/=> new RefreshToken();


    }
}
