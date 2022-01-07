using GloboTicket.TicketManagement.Mongo.Identity.UnitTests.Fakes;
using GloboTicket.TicketManagement.mongo.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Mongo.Identity.UnitTests.Mocks
{
    public class RoleManagerMocks
    {
        public static Mock<FakeRoleManager> GetRoleManager()
        {
            var Roles = new List<ApplicationRole>();
            var mockRoleManager = new Mock<FakeRoleManager>();

            mockRoleManager.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync((ApplicationRole role) =>
            {
                if (role == null)
                    return IdentityResult.Failed(new List<IdentityError>().ToArray());
                Roles.Add(role);
                return IdentityResult.Success;
            });

            return mockRoleManager;
        }
    }
}
