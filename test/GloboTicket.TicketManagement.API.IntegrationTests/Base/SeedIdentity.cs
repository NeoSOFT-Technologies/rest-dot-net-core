using GloboTicket.TicketManagement.Identity;
using GloboTicket.TicketManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.API.IntegrationTests.Base
{
    public class SeedIdentity
    {
        private readonly GloboTicketIdentityDbContext _identityDbContext;

        public SeedIdentity(GloboTicketIdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }
        public async void SeedUsers()
        {
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

            ApplicationUser viewer = new ApplicationUser()
            {
                FirstName = "Apoorv",
                LastName = "Rame",
                UserName = "apoorv",
                NormalizedUserName = "APOORV",
                Email = "apoorv@test.com",
                NormalizedEmail = "APOORV@TEST.COM",
                EmailConfirmed = true
            };
            viewer.PasswordHash = passwordHasher.HashPassword(viewer, "User123!@#");
            await _identityDbContext.Users.AddAsync(viewer);

            await _identityDbContext.SaveChangesAsync();
        }

        public async void SeedUserRoles()
        {
            var roles = await _identityDbContext.Roles.ToListAsync();
            var users = await _identityDbContext.Users.ToListAsync();

            for (int i = 0; i < roles.Count; i++)
            {
                var userRole = new IdentityUserRole<string>() { RoleId = roles[i].Id, UserId = users[i].Id };
                await _identityDbContext.UserRoles.AddAsync(userRole);
            }
            await _identityDbContext.SaveChangesAsync();
        }
    }
}
