using StackExchange.Redis;

namespace RedisDemo.Data.Cache
{
    public static class LocalRedisCacheConnector
    {
        public static ConnectionMultiplexer Multiplexer => _lazyMultiplexer.Value;

        private static readonly Lazy<ConnectionMultiplexer> _lazyMultiplexer = new Lazy<ConnectionMultiplexer>(() =>
        {
            var config = GetCacheConfiguration();
            var redisConnection = ConnectionMultiplexer.Connect(config);
            return redisConnection;
        });

        private static ConfigurationOptions GetCacheConfiguration(int connectionTimeout = 5000, int syncTimeout = 3000)
        {
            var configuration = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                AllowAdmin = true,
                ConnectTimeout = connectionTimeout,
                SyncTimeout = syncTimeout,
                //Password = "secret"
            };
            configuration.EndPoints.Add("localhost", 6379);

            return configuration;
        }
    }
}
