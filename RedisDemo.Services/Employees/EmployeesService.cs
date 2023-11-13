using Microsoft.Extensions.Caching.Memory;
using RedisDemo.Models.AdventureWorks;
using RedisDemo.Models.Cache;
using RedisDemo.Models.Repositories;

namespace RedisDemo.Services.Employees
{
    public class EmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IDistributedCacheService _cacheRepository;
        private readonly IExtendedCacheRepository<Employee> _extendedCacheRepository;
        private readonly IMemoryCache _memoryCache;

        public EmployeesService(IEmployeesRepository employeesRepository,
            IDistributedCacheService cacheRepository,
            IExtendedCacheRepository<Employee> extendedCacheRepository,
            IMemoryCache memoryCache)
        {
            _employeesRepository = employeesRepository;
            _cacheRepository = cacheRepository;
            _extendedCacheRepository = extendedCacheRepository;
            _memoryCache = memoryCache;
        }

        public async Task<ICollection<Employee>> GetAllAsync()
        {
            return await _employeesRepository.GetAllAsync();
        }

        public async Task<ICollection<Employee>> GetAllFromLocalCacheAsync()
        {
            var employeesCacheKey = $"employees";
            var employees = await _memoryCache.GetOrCreateAsync(employeesCacheKey, async (cacheEntry) =>
            {
                return await _employeesRepository.GetAllAsync();
            });
            return employees;
        }

        public async Task<ICollection<Employee>> GetAllFromCacheAsync()
        {
            var employeesCacheKeyPattern = "employee_*";
            var employees = await _extendedCacheRepository.GetAllAsync(employeesCacheKeyPattern);
            if (employees?.Any() != true)
            {
                employees = await GetAllAsync();

                if (employees?.Any() == true)
                {
                    var cacheKeys = employees.ToDictionary(e => $"employee_logid_{e.LoginId}", e => e);
                    await _extendedCacheRepository.SetKeysAsync(cacheKeys);
                }
            }

            return employees;
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
                cacheEntry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(1);
                return await GetByLoginIdAsync(loginId);
            });

            return employee;
        }
    }
}
