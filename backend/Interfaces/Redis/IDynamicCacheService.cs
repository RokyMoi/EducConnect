using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Interfaces.Redis
{
    public interface IDynamicCacheService
    {
        Task<T> GetOrAddCache<T>(string key, Func<Task<T>> factory, int usageThreshold = 3, TimeSpan? expiration = null);
    }
}