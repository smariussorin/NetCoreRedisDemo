using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisDemo.Models.Repositories;

namespace RedisDemo.Data.Cache
{
    public class DistributedCacheService: IDistributedCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            string cacheValue = await _distributedCache.GetStringAsync(key);

            if (string.IsNullOrWhiteSpace(cacheValue))
            {
                return default;
            }

            T value = JsonConvert.DeserializeObject<T>(cacheValue);
            return value;

        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T value) where T : class
        {
            string cacheValue = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(key, cacheValue);
        }
    }
}
