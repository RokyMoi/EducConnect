using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Interfaces.Redis
{
    public interface IRedisCachingService
    {
        Task SetCache<T>(string cacheKey, T data, TimeSpan? expiration = null);
        Task<T?> GetCache<T>(string cacheKey, TimeSpan? expiration = null);
    }
}