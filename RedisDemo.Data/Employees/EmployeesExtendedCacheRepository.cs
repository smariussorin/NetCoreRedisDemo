using RedisDemo.Data.Cache;
using RedisDemo.Models.AdventureWorks;

namespace RedisDemo.Data.Employees
{
    public class EmployeesExtendedCacheRepository : ExtendedCacheRedisRepository<Employee>
    {
        public override int DbId => 1;
    }
}
