using Microsoft.Extensions.Caching.Memory;

namespace Cryptocurrency.Api.Application
{
    public class CryptoCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(15);

        public CryptoCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Method to get data from cache or fetch it and cache it
        public async Task<string> GetCryptoCodeAsync()
        {
            _cache.TryGetValue("Symbols", out string cachedResult);

            return cachedResult;
        }

        public void Set(string cryptoCode)
        {
            _cache.TryGetValue("Symbols", out string cachedResult);
            if (!string.IsNullOrEmpty(cachedResult))
                cachedResult += "," + cryptoCode;
            else
                cachedResult = cryptoCode;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cacheExpiration);

            _cache.Set("Symbols", cachedResult, cacheEntryOptions);
        }
    }
}
