using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RedisDemo.Models.Repositories;
using RedisDemo.Data.Cache;
using RedisDemo.Data.Employees;
using RedisDemo.Data.Model;
using RedisDemo.Services.Employees;
using StackExchange.Redis;
using RedisDemo.Models.AdventureWorks;
using RedisDemo.Models.Cache;

namespace RedisDemo.CompositionRoot
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.ConfigureConnections(configuration)
                    .ConfigureBusinessLogic()
                    .ConfigureDataLayer();
        }

        private static IServiceCollection ConfigureConnections(this IServiceCollection services,
               IConfiguration configuration)
        {
            var dbServer = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AdventureWorksContext>(options =>
                options.UseSqlServer(dbServer));

            var cacheServer = configuration.GetConnectionString("CacheConnection");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheServer;
                options.InstanceName = "DemoInstance_";
            });
            services.AddDistributedMemoryCache();

            //Self configuring cache
            //services.AddSingleton<IDistributedCache, RedisCache>(sp =>
            //{
            //    var options = new RedisCacheOptions
            //    {
            //        ConfigurationOptions = new ConfigurationOptions
            //        {
            //            AbortOnConnectFail = false,
            //            AllowAdmin = true,
            //            Password = "secret",
            //            ConnectTimeout = 5000,
            //            SyncTimeout = 3000,
            //        }
            //    };
            //    options.ConfigurationOptions.EndPoints.Add(configuration[cacheServer]);

            //    // Set the options for this instance
            //    return new RedisCache(Options.Create(options));
            //});

            return services;
        }
        private static IServiceCollection ConfigureBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<EmployeesService, EmployeesService>();

            return services;
        }

        private static IServiceCollection ConfigureDataLayer(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IDistributedCacheService, DistributedCacheService>();

            services.AddScoped<IEmployeesRepository, EmployeesRepository>();

            services.AddScoped<IExtendedCacheRepository<Employee>, EmployeesExtendedCacheRepository>();
            
            return services;
        }
    }
}
