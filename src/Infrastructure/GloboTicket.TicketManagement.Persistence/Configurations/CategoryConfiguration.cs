using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GloboTicket.TicketManagement.Domain.Entities;

namespace Scrum.Demo.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Not necessary if naming conventions are followed in model
            builder
                .HasKey(b => b.CategoryId);

            builder
                .Property(b => b.CreatedBy)
                .HasColumnType("nvarchar(450)");

            builder
                .Property(b => b.LastModifiedBy)
                .HasColumnType("nvarchar(450)");

            builder
                .Property(b => b.Name)
                .IsRequired()
                .HasColumnType("nvarchar(50)");
        }
    }
}
