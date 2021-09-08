using GloboTicket.TicketManagement.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Identity
{
    public class GloboTicketIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public GloboTicketIdentityDbContext(DbContextOptions<GloboTicketIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(GloboTicketIdentityDbContext).Assembly);
        }
    }
}
