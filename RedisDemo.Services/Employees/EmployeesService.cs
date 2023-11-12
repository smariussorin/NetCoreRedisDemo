using Microsoft.Extensions.Caching.Memory;
using RedisDemo.Models.AdventureWorks;
using RedisDemo.Models.Repositories;

namespace RedisDemo.Services.Employees
{
    public class EmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IDistributedCacheService _cacheRepository;
        private readonly IMemoryCache _memoryCache;

        public EmployeesService(IEmployeesRepository employeesRepository, IDistributedCacheService cacheRepository, IMemoryCache memoryCache)
        {
            _employeesRepository = employeesRepository;
            _cacheRepository = cacheRepository;
            _memoryCache = memoryCache;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _employeesRepository.GetAllAsync();
        }

        public async Task<Employee> GetByLoginIdAsync(string loginId)
        {
            if (string.IsNullOrWhiteSpace(loginId))
            {
                throw new ArgumentNullException(nameof(loginId));
            }

            return await _employeesRepository.GetByLoginIdAsync(loginId);
        }

        public async Task<Employee> GetByLoginIdFromCacheAsync(string loginId)
        {
            if (string.IsNullOrWhiteSpace(loginId))
            {
                throw new ArgumentNullException(nameof(loginId));
            }

            var employeeCacheKey = $"employee_logid_{loginId}";
            var employee = await _cacheRepository.GetAsync<Employee>(employeeCacheKey);
            if (employee == null)
            {
                employee = await GetByLoginIdAsync(loginId);

                await _cacheRepository.SetAsync(employeeCacheKey, employee);
            }

            return employee;
        }

        public async Task<Employee> GetByLoginIdFromLocalCacheAsync(string loginId)
        {
            if (string.IsNullOrWhiteSpace(loginId))
            {
                throw new ArgumentNullException(nameof(loginId));
            }

            var employeeCacheKey = $"employee_logid_{loginId}";
            var employee = await _memoryCache.GetOrCreateAsync(employeeCacheKey, async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(5);

                return await GetByLoginIdAsync(loginId);
            });

            return employee;
        }
    }
}
