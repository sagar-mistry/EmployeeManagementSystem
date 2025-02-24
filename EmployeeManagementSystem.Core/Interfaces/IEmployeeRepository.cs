using EmployeeManagementSystem.Core.Enitities;

namespace EmployeeManagementSystem.Core.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeesAsync(int pageNumber, int pageSize);
    Task<Employee> GetEmployeeByIdAsync(int employeeNumber);
    Task<IEnumerable<Employee>> SearchEmployeesAsync(string name);
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int employeeNumber);
}