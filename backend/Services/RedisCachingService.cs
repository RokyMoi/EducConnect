using System;
using System.Text.Json;
using System.Threading.Tasks;
using EduConnect.Interfaces.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace EduConnect.Services
{
    public class RedisCachingService(IDistributedCache cache, ILogger<IRedisCachingService> logger) : IRedisCachingService
    {
        private readonly IDistributedCache _cache = cache;
        private readonly ILogger<IRedisCachingService> _logger = logger;

        public async Task<T?> GetCache<T>(string cacheKey, TimeSpan? expiration = null)
        {
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData == null)
            {
                _logger.LogInformation($"Cache miss - Key: {cacheKey}");
                return default(T);
            }

            _logger.LogInformation($"Cache hit - Key: {cacheKey}");
            return JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetCache<T>(string cacheKey, T data, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize<T>(data);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };

            await _cache.SetStringAsync(cacheKey, json, options);
        }
    }
}