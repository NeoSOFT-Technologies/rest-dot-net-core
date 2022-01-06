using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.mongo.Identity.Models;

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Identity.Settings
{
    public class ContextSeedData
    {
        public static void SeedData(IMongoCollection<ApplicationUser> userCollection)
        {
            bool existUser = userCollection.Find(p => true).Any();
            if (!existUser)
            {
                userCollection.InsertManyAsync(GetPreconfiguredUser());

            }
        }
        private static IEnumerable<ApplicationUser> GetPreconfiguredUser()
        {
            var concertGuid = new string("61cc58c88b8879cc049839a8");
            var musicalGuid = new string("61d1cc89ace80d15f6249a53");

            List<ApplicationUser> appUser = new List<ApplicationUser>();
            
                
                PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

                ApplicationUser admin = new ApplicationUser()
                {
                    FirstName = "John",
                    LastName = "Smith",
                    UserName = "johnsmith",
                    NormalizedUserName = "JOHNSMITH",
                    Email = "john@test.com",
                    NormalizedEmail = "JOHN@TEST.COM",
                    EmailConfirmed = true
                };
                admin.PasswordHash = passwordHasher.HashPassword(admin, "User123!@#");
                await _identityDbContext.Users.AddAsync(admin);

            };
        
    }
}
