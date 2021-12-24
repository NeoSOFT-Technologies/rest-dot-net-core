using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace GloboTicket.TicketManagement.Infrastructure.UnitTests.Mocks
{
    public class MemoryCacheMocks
    {
        public static Mock<IMemoryCache> GetMemoryCache(object expectedValue)
        {
            var mockMemoryCache = new Mock<IMemoryCache>();

            mockMemoryCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out expectedValue)).Returns(true);
            mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);
            mockMemoryCache.Setup(x => x.Remove(It.IsAny<string>()));

            return mockMemoryCache;
        }
    }
}
