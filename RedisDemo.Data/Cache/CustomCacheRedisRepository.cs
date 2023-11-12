using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;

namespace RedisDemo.Data.Cache
{
    public abstract partial class ExtendedCacheRedisRepository<T>
    {
        public virtual int DbId { get; } = 0;

        protected IDatabase Database => ConnectionMultiplexer.GetDatabase(DbId);

        public virtual ConnectionMultiplexer ConnectionMultiplexer => LocalRedisCacheConnector.Multiplexer;

        public async Task<bool> ContainsKeyAsync(string key)
        {
            return await Database.KeyExistsAsync(key);
        }

        public async Task<T> GetValueAsync(string key)
        {
            var redisValue = await Database.StringGetAsync(key);
            if (redisValue.HasValue)
            {
                return DeserializeObject<T>(redisValue);
            }
            return default;
        }

        public async Task<ICollection<T>> GetAllAsync(string pattern)
        {
            var result = new List<T>();
            RedisValue[] redisValues = null;
            var keysResult = await SearchKeysAsync(Database, pattern);
            var keys = keysResult.ToArray();
            if (keys?.Any() == true)
            {
                var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
                redisValues = await Database.StringGetAsync(redisKeys, CommandFlags.None);
            }
            if (redisValues != null)
            {
                foreach (var value in redisValues)
                {
                    if (value != RedisValue.Null)
                    {
                        var res = DeserializeObject<T>(value);
                        result.Add(res);
                    }
                }
            }
            return result;
        }

        public async Task<ICollection<T>> GetValuesForKeysAsync(ICollection<string> keys)
        {
            List<T> results = new List<T>();

            var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
            var redisValues = await Database.StringGetAsync(redisKeys);
            if (redisValues != null)
            {
                foreach (var value in redisValues)
                {
                    if (value != RedisValue.Null)
                    {
                        var result = DeserializeObject<T>(value);
                        results.Add(result);
                    }
                }
            }

            return results;
        }

        public async Task SetKeyAsync(string key, T value)
        {
            var stringValue = SerializeObject(value);
            await Database.StringSetAsync(key, stringValue);
        }

        public async Task SetKeysAsync(Dictionary<string, T> data)
        {
            var serializedData = SerializeObject(data);
            await Database.StringSetAsync(serializedData);
        }

        public async Task ClearKeyLocalAsync(string key)
        {
            await Database.KeyDeleteAsync(key);
        }

        public async Task ClearKeysLocalAsync(IEnumerable<string> keys)
        {
            if (keys?.Any() == true)
            {
                var redisKeys = keys.Select(key => new RedisKey(key)).ToArray();
                await Database.KeyDeleteAsync(redisKeys);
            }
        }

        private async Task<List<string>> SearchKeysAsync(IDatabaseAsync database, string filter)
        {
            var results = new List<string>();
            int nextCursor = 0;
            do
            {
                RedisResult redisResult = await database.ExecuteAsync("SCAN", new object[] { nextCursor.ToString(), "MATCH", filter, "COUNT", "1000" });
                var innerResult = (RedisResult[])redisResult;

                nextCursor = int.Parse((string)innerResult[0]);

                List<string> resultLines = ((string[])innerResult[1]).ToList();
                results.AddRange(resultLines);
            }
            while (nextCursor != 0);
            return results;
        }

        protected byte[] SerializeObject<U>(U value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            var valueBytes = Encoding.UTF8.GetBytes(jsonString);
            return valueBytes;
        }

        protected KeyValuePair<RedisKey, RedisValue>[] SerializeObject<U>(Dictionary<string, U> data)
        {
            var serializedDataArray = data.Select(x =>
                new KeyValuePair<RedisKey, RedisValue>(x.Key, JsonConvert.SerializeObject(x.Value))
            ).ToArray();
            return serializedDataArray;
        }

        protected U DeserializeObject<U>(RedisValue redisValue)
        {
            var valueString = Encoding.UTF8.GetString(redisValue);
            var value = JsonConvert.DeserializeObject<U>(valueString);
            return value;
        }
    }
}