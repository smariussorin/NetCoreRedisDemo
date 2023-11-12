using RedisDemo.Models.AdventureWorks;

namespace RedisDemo.Models.Repositories
{
    public interface IEmployeesRepository
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee> GetByLoginIdAsync(string loginId);
    }
}