﻿using Microsoft.EntityFrameworkCore;
using RedisDemo.Models.AdventureWorks;
using RedisDemo.Models.Repositories;
using RedisDemo.Data.Model;

namespace RedisDemo.Data.Employees
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly AdventureWorksContext _dbContext;

        public EmployeesRepository(AdventureWorksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employee>> GetAllAsync(int top = 1000)
        {
            return await _dbContext.Employees.Take(top).ToListAsync();
        }

        public async Task<Employee> GetByLoginIdAsync(string loginId)
        {
            return await _dbContext.Employees.Where(e => e.LoginId.ToLower() == loginId.ToLower()).FirstOrDefaultAsync();
        }
    }
}
