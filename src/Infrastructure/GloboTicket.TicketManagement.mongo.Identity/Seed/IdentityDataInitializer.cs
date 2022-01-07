using GloboTicket.TicketManagement.mongo.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.mongo.Identity.Seed
{
    public class IdentityDataInitializer
    {
        public static void SeedData(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("NancyDavolio").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.FirstName = "Nancy";
                user.LastName = "Davolio";
                user.Email = "user1@test.com";
                user.UserName = "NancyDavolio";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "User123!@#").Result;



                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Viewer").Wait();
                }
            }


            if (userManager.FindByNameAsync
        ("johnsmith").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "johnsmith";
                user.Email = "john@test.com";
                user.FirstName = "John";
                user.LastName = "Smith";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "User123!@#").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,"Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Viewer").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Viewer";
                role.NormalizedName = "VIEWER";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync
        ("Administrator").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Administrator";
                role.NormalizedName = "ADMINISTRATOR";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
