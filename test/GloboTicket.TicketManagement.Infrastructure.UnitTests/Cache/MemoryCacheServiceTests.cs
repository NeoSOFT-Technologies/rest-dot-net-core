using GloboTicket.TicketManagement.Application.Models.Cache;
using GloboTicket.TicketManagement.Infrastructure.Cache;
using GloboTicket.TicketManagement.Infrastructure.UnitTests.Mocks;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace GloboTicket.TicketManagement.Infrastructure.UnitTests.Cache
{
    public class MemoryCacheServiceTests
    {
        private readonly IOptions<CacheConfiguration> _cacheOptions;
        public MemoryCacheServiceTests()
        {
            _cacheOptions = Options.Create(new CacheConfiguration()
            {
                AbsoluteExpirationInHours = 1,
                SlidingExpirationInMinutes = 30
            });
        }

        [Fact]
        public void TryGet_Null_Value()
        {
            object expectedValue = null;
            var mockMemoryCahe = MemoryCacheMocks.GetMemoryCache(expectedValue);
            var service = new MemoryCacheService(mockMemoryCahe.Object, _cacheOptions);

            var result = service.TryGet("key", out expectedValue);

            result.ShouldBeOfType<bool>();
            result.ShouldBe(false);
        }

        [Fact]
        public void TryGet_NotNull_Value()
        {
            object expectedValue = "value";
            var mockMemoryCahe = MemoryCacheMocks.GetMemoryCache(expectedValue);
            var service = new MemoryCacheService(mockMemoryCahe.Object, _cacheOptions);

            var result = service.TryGet("key", out expectedValue);

            result.ShouldBeOfType<bool>();
            result.ShouldBe(true);
        }

        [Fact]
        public void Set()
        {
            object expectedValue = "value";
            var mockMemoryCahe = MemoryCacheMocks.GetMemoryCache(expectedValue);
            var service = new MemoryCacheService(mockMemoryCahe.Object, _cacheOptions);

            var result = service.Set("key", expectedValue);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<string>();
        }

        [Fact]
        public void Remove()
        {
            object expectedValue = "value";
            var mockMemoryCahe = MemoryCacheMocks.GetMemoryCache(expectedValue);
            var service = new MemoryCacheService(mockMemoryCahe.Object, _cacheOptions);

            Should.NotThrow(() => service.Remove("key"));
        }
    }
}
