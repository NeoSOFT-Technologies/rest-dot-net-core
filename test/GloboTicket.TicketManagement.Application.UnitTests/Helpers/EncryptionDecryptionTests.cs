using GloboTicket.TicketManagement.Infrastructure.EncryptDecrypt;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Application.UnitTests.Helpers
{
    public class EncryptionDecryptionTests
    {
        [Fact]
        public async Task EncryptDecrypt()
        {
            string originalString = "Test";

            string encryptedString = EncryptionDecryption.EncryptString(originalString, "MAKV2SPBNI99212");
            string decryptedString = EncryptionDecryption.DecryptString(encryptedString, "MAKV2SPBNI99212");

            decryptedString.ShouldBeEquivalentTo(originalString);
        }
    }
}
