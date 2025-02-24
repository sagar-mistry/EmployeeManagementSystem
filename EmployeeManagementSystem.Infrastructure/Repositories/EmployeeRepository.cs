using EmployeeManagementSystem.Core.Enitities;
using EmployeeManagementSystem.Core.Interfaces;
using EmployeeManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _context;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(EmployeeDbContext context, ILogger<EmployeeRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation("GetEmployeesAsync called. PageNumber: {PageNumber}, PageSize: {PageSize}", pageNumber, pageSize);
        var employees = await _context.Employees.AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        _logger.LogInformation("GetEmployeesAsync completed successfully. Found {EmployeeCount} employees.", employees.Count);
        return employees;
    }

    public async Task<Employee> GetEmployeeByIdAsync(int employeeNumber)
    {
        _logger.LogInformation("GetEmployeeByIdAsync called. EmployeeNumber: {EmployeeNumber}", employeeNumber);
        var employee = await _context.Employees.FindAsync(employeeNumber);
        if (employee == null)
        {
            _logger.LogWarning("GetEmployeeByIdAsync. Employee with EmployeeNumber: {EmployeeNumber} not found", employeeNumber);
        }

        _logger.LogInformation("GetEmployeeByIdAsync completed successfully. EmployeeNumber: {EmployeeNumber}", employeeNumber);
        return employee;

    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        _logger.LogInformation("AddEmployeeAsync called. EmployeeNumber: {EmployeeNumber}", employee.EmployeeNumber);
        employee.TotalPay = employee.HourlyRate * employee.HoursWorked;
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        _logger.LogInformation("AddEmployeeAsync completed successfully. EmployeeNumber: {EmployeeNumber}", employee.EmployeeNumber);
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _logger.LogInformation("UpdateEmployeeAsync called. EmployeeNumber: {EmployeeNumber}", employee.EmployeeNumber);
        _context.Entry(employee).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        _logger.LogInformation("UpdateEmployeeAsync completed successfully. EmployeeNumber: {EmployeeNumber}", employee.EmployeeNumber);

    }

    public async Task DeleteEmployeeAsync(int employeeNumber)
    {
        _logger.LogInformation("DeleteEmployeeAsync called. EmployeeNumber: {EmployeeNumber}", employeeNumber);
        var employee = await _context.Employees.FindAsync(employeeNumber);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            _logger.LogInformation("DeleteEmployeeAsync completed successfully. EmployeeNumber: {EmployeeNumber}", employeeNumber);
        }
        else
        {
            _logger.LogWarning("DeleteEmployeeAsync. Employee with EmployeeNumber: {EmployeeNumber} not found", employeeNumber);
        }

    }

    public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string name)
    {

        _logger.LogInformation("Repository: Searching Employees. SearchTerm: {SearchTerm}", name);

        var employees = await _context.Employees
            .Where(e => EF.Functions.Like(e.EmployeeName, $"%{name}%")) // Case-insensitive search
            .Select(e => new Employee
            {
                EmployeeNumber = e.EmployeeNumber,
                EmployeeName = e.EmployeeName,
                HourlyRate = e.HourlyRate,
                HoursWorked = e.HoursWorked
            })
            .ToListAsync();

        _logger.LogInformation("Repository: Search completed. Found {EmployeeCount} employees.", employees.Count());

        return employees;
    }
}

