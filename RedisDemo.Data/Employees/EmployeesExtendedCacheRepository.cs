using RedisDemo.Data.Cache;
using RedisDemo.Models.AdventureWorks;
using RedisDemo.Models.Repositories;

namespace RedisDemo.Data.Employees
{
    public class EmployeesExtendedCacheRepository : ExtendedCacheRedisRepository<Employee>, IEmployeesExtendedCacheRepository
    {
        public override int DbId => 1;
    }
}
