using AspNetCore.Identity.MongoDbCore.Models;
using GloboTicket.TicketManagement.mongo.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;*/
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.mongo.Identity.Configurations
{
    [ExcludeFromCodeCoverage]
    public class RoleConfiguration
    {
        /*: IEntityTypeConfiguration<*//*IdentityRole*//*MongoIdentityRole>
    {
        public void Configure(EntityTypeBuilder<*//*IdentityRole*//*MongoIdentityRole> builder)
        {
            builder.HasData(
                new MongoIdentityRole
                {
                    Name = "Viewer",
                    NormalizedName = "VIEWER"
                },
                new MongoIdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
            );
        }*/

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<MongoIdentityRole>>();


            IdentityResult roleResult;
            //Adding Addmin Role  
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                roleResult = await RoleManager.CreateAsync(new MongoIdentityRole("Admin"));
            }

            roleCheck = await RoleManager.RoleExistsAsync("Viewer");
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                roleResult = await RoleManager.CreateAsync(new MongoIdentityRole("Viewer"));
            }


        }
}
}
