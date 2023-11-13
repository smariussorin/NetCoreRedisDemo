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

            if (cacheValue == null)
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

            //Example to add expire options
            //var options = new DistributedCacheEntryOptions(); // create options object
            //options.SetSlidingExpiration(TimeSpan.FromSeconds(10)); // 10 seconds sliding expiration

            await _distributedCache.SetStringAsync(key, cacheValue);
        }
    }
}
