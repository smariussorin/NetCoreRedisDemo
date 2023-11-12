using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisDemo.Models.AdventureWorks;
using RedisDemo.Data.Model;
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
        public async Task<Employee> GetByLoginId(string loginId)
        {
            return await _employeeService.GetByLoginIdAsync(loginId);
        }

        [HttpGet]
        public async Task<Employee> GetByLoginIdFromCache(string loginId)
        {
            return await _employeeService.GetByLoginIdFromCacheAsync(loginId);
        }
    }
}