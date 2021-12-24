using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GloboTicket.TicketManagement.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Scrum.Demo.Persistence.Configurations
{
    [ExcludeFromCodeCoverage]
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
                .HasColumnType("varchar(450)");

            builder
                .Property(b => b.LastModifiedBy)
                .HasColumnType("varchar(450)");
        }
    }
}
