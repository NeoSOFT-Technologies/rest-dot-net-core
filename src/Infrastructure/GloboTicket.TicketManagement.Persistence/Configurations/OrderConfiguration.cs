using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GloboTicket.TicketManagement.Domain.Entities;

namespace Scrum.Demo.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //Not necessary if naming conventions are followed in model
            builder
                .HasKey(b => b.Id);

            builder
                .Property(b => b.Id)
                .HasColumnName("OrderId");

            builder
                .Property(b => b.CreatedBy)
                .HasColumnType("nvarchar(450)");

            builder
                .Property(b => b.LastModifiedBy)
                .HasColumnType("nvarchar(450)");
        }
    }
}
