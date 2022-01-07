/*using GloboTicket.TicketManagement.Identity.Seed;*/
using GloboTicket.TicketManagement.Mongo.Identity.UnitTests.Fakes;
using GloboTicket.TicketManagement.Mongo.Identity.UnitTests.Mocks;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using GloboTicket.TicketManagement.mongo.Identity.Seed;

namespace GloboTicket.TicketManagement.Mongo.Identity.UnitTests.Seed
{
    public class UserCreatorTests
    {
        private readonly static Mock<FakeUserManager> _mockUserManager;
        static UserCreatorTests()
        {
            _mockUserManager = UserManagerMocks.GetUserManager();
        }

        [Fact]
        public async static Task Create_First_User()
        {
            var oldCount = _mockUserManager.Object.Users.ToList().Count();

            await Should.NotThrowAsync(() => UserCreator.SeedAsync(_mockUserManager.Object));

            var newCount = _mockUserManager.Object.Users.ToList().Count();
            newCount.ShouldBe(oldCount + 1);
        }
    }
}
