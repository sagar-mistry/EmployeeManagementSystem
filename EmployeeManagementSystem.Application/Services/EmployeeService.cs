using AutoMapper;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Core.Enitities;
using EmployeeManagementSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("GetEmployeesAsync called. PageNumber: {PageNumber}, PageSize: {PageSize}", pageNumber, pageSize);
            var employees = await _employeeRepository.GetEmployeesAsync(pageNumber, pageSize);
            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            _logger.LogInformation("GetEmployeesAsync completed successfully. Found {EmployeeCount} employees.", employeeDTOs.Count());
            return employeeDTOs;

        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int employeeNumber)
        {
            _logger.LogInformation("GetEmployeeByIdAsync called. EmployeeNumber: {EmployeeNumber}", employeeNumber);
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeNumber);
            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);

            _logger.LogInformation("GetEmployeeByIdAsync completed successfully. EmployeeNumber: {EmployeeNumber}", employeeNumber);
            return employeeDTO;
        }

        public async Task AddEmployeeAsync(EmployeeDTO employeeDTO)
        {
            _logger.LogInformation("AddEmployeeAsync called. Employee: {@EmployeeDTO}", employeeDTO);
            var employee = _mapper.Map<Employee>(employeeDTO);
            await _employeeRepository.AddEmployeeAsync(employee);

            _logger.LogInformation("AddEmployeeAsync completed successfully. EmployeeNumber: {EmployeeNumber}", employee.EmployeeNumber);
        }

        public async Task UpdateEmployeeAsync(EmployeeDTO employeeDTO)
        {
            _logger.LogInformation("UpdateEmployeeAsync called. Employee: {@EmployeeDTO}", employeeDTO);
            var employee = _mapper.Map<Employee>(employeeDTO);
            await _employeeRepository.UpdateEmployeeAsync(employee);
            _logger.LogInformation("UpdateEmployeeAsync completed successfully. EmployeeNumber: {EmployeeNumber}", employee.EmployeeNumber);

        }

        public async Task DeleteEmployeeAsync(int employeeNumber)
        {
            _logger.LogInformation("DeleteEmployeeAsync called. EmployeeNumber: {EmployeeNumber}", employeeNumber);
            await _employeeRepository.DeleteEmployeeAsync(employeeNumber);
            _logger.LogInformation("DeleteEmployeeAsync completed successfully. EmployeeNumber: {EmployeeNumber}", employeeNumber);
        }

        public async Task<IEnumerable<EmployeeDTO>> SearchEmployeesAsync(string name)
        {
            _logger.LogInformation("SearchEmployeesAsync called. EmployeeName: {EmployeeName}", name);
            var employees = await _employeeRepository.SearchEmployeesAsync(name);
            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
            _logger.LogInformation("SearchEmployeesAsync completed successfully. EmployeeName: {EmployeeName}", name);
            return employeeDTOs;
        }
    }
}