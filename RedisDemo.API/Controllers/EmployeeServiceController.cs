using Microsoft.AspNetCore.Mvc;
using RedisDemo.Models.AdventureWorks;
using RedisDemo.Services.Employees;

namespace RedisDemo.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeesService _employeeService;

        public EmployeesController(EmployeesService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _employeeService.GetAllAsync();
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> GetAllFromLocalCache()
        {
            return await _employeeService.GetAllFromLocalCacheAsync();
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> GetAllFromCache()
        {
            return await _employeeService.GetAllFromCacheAsync();
        }

        [HttpGet]
        public async Task<Employee> GetByLoginId(string loginId)
        {
            return await _employeeService.GetByLoginIdAsync(loginId);
        }

        [HttpGet]
        public async Task<Employee> GetByLoginIdFromLocalCache(string loginId)
        {
            return await _employeeService.GetByLoginIdFromLocalCacheAsync(loginId);
        }

        [HttpGet]
        public async Task<Employee> GetByLoginIdFromCache(string loginId)
        {
            return await _employeeService.GetByLoginIdFromCacheAsync(loginId);
        }
    }
}