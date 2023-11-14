using RedisDemo.Models.AdventureWorks;

namespace RedisDemo.Models.Repositories
{
    public interface IEmployeesRepository
    {
        Task<List<Employee>> GetAllAsync(int top = 1000);

        Task<Employee> GetByLoginIdAsync(string loginId);
    }
}