using EmployeeManagementSystem.Application.DTOs;

namespace EmployeeManagementSystem.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync(int pageNumber, int pageSize);
    Task<EmployeeDTO> GetEmployeeByIdAsync(int employeeNumber);
    Task AddEmployeeAsync(EmployeeDTO employeeDTO);
    Task UpdateEmployeeAsync(EmployeeDTO employeeDTO);
    Task DeleteEmployeeAsync(int employeeNumber);
}