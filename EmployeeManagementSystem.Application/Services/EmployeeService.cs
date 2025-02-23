using AutoMapper;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Core.Enitities;
using EmployeeManagementSystem.Core.Interfaces;


namespace EmployeeManagementSystem.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync(int pageNumber, int pageSize)
        {
            var employees = await _employeeRepository.GetEmployeesAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int employeeNumber)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeNumber);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task AddEmployeeAsync(EmployeeDTO employeeDTO)
        {
            var employee = _mapper.Map<Employee>(employeeDTO);
            await _employeeRepository.AddEmployeeAsync(employee);
        }

        public async Task UpdateEmployeeAsync(EmployeeDTO employeeDTO)
        {
            var employee = _mapper.Map<Employee>(employeeDTO);
            await _employeeRepository.UpdateEmployeeAsync(employee);
        }

        public async Task DeleteEmployeeAsync(int employeeNumber)
        {
            await _employeeRepository.DeleteEmployeeAsync(employeeNumber);
        }
    }
}