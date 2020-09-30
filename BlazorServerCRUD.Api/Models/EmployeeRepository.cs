using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorServerCRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerCRUD.Api.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _appDbContext.Employees
                            .Include(e => e.Department)
                            .ToListAsync();
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            return await _appDbContext.Employees
                            .Include(e => e.Department)
                            .FirstOrDefaultAsync(e => e.EmployeeID == employeeId);
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            var result = await _appDbContext.Employees.AddAsync(employee);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var result = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeID == employee.EmployeeID);

            if (result != null)
            {
                result.EmployeeName = employee.EmployeeName;
                result.Gender = employee.Gender;
                result.DepartmentID = employee.DepartmentID;
                result.DateOfBirth = employee.DateOfBirth;
            }

            return result;
        }

        public async Task<Employee> DeleteEmployee(int employeeId)
        {
            var employee = await _appDbContext.Employees.FindAsync(employeeId);

            if (employee != null)
            {
                _appDbContext.Employees.Remove(employee);
                await _appDbContext.SaveChangesAsync();
            }
            
            return employee;
        }
    }
}