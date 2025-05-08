using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EduConnect.Interfaces.Redis;
using Microsoft.Extensions.Caching.Distributed;


namespace EduConnect.Services
{
    public class DynamicCacheService(IDistributedCache cache, ILogger<DynamicCacheService> logger) : IDynamicCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<DynamicCacheService> _logger;
        public async Task<T> GetOrAddCache<T>(string key, Func<Task<T>> factory, int usageThreshold = 3, TimeSpan? expiration = null)
        {
            var cached = await _cache.GetStringAsync(key);

            if (cached != null)
            {
                _logger.LogInformation($"Cache hit - Key: {key}");
                return JsonSerializer.Deserialize<T>(cached);
            }

            _logger.LogInformation($"Cache miss - Key: {key} - loading fresh data");
            var result = await factory();

            var json = JsonSerializer.Serialize<T>(result);

            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            });

            return result;
        }

    }
}