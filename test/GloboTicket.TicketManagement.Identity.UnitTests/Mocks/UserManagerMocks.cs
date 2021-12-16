using GloboTicket.TicketManagement.Identity.Models;
using GloboTicket.TicketManagement.Identity.UnitTests.Fakes;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace GloboTicket.TicketManagement.Identity.UnitTests.Mocks
{
    public class UserManagerMocks
    {
        public static Mock<FakeUserManager> GetUserManager()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "Test",
                    Id = "01c3479f-9038-4a8a-b684-cb07ee66f34c",
                    Email = "test@test.it",
                    RefreshTokens = new List<RefreshToken>()
                                {
                                    new RefreshToken()
                                    {
                                        Token = "Token",
                                        Revoked = null,
                                        Expires = DateTime.UtcNow.AddDays(1)
                                    }
                                }
                },
                new ApplicationUser
                {
                    UserName = "Test1",
                    Id = "bfafae22-d2d3-4085-b657-f7ca580c1d44",
                    Email = "test1@test.it",
                    RefreshTokens = new List<RefreshToken>()
                                {
                                    new RefreshToken()
                                    {
                                        Token = "ExpiredToken",
                                        Revoked = DateTime.UtcNow,
                                        Expires = DateTime.UtcNow.AddDays(1)
                                    }
                                }
                }
            };

            var mockUserManager = new Mock<FakeUserManager>();

            mockUserManager.Setup(x => x.Users)
                .Returns(users.AsQueryable());
            mockUserManager.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<Claim>());
            mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string>() { "Viewer" });
            mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) =>
                {
                    return users.FirstOrDefault(x => x.Email == email);
                });

            mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string userName) =>
                {
                    return users.FirstOrDefault(x => x.UserName == userName);
                });

            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).Callback(
                (ApplicationUser user) =>
                {
                    var oldUser = users.First(x => x.Id == user.Id);
                    var index = users.IndexOf(oldUser);
                    if (index != -1)
                        users[index] = user;
                });

            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser user, string password) =>
                {
                    if (password == null)
                        return IdentityResult.Failed(new List<IdentityError>().ToArray());
                    users.Add(user);
                    return IdentityResult.Success;
                });

            return mockUserManager;
        }
    }
}
