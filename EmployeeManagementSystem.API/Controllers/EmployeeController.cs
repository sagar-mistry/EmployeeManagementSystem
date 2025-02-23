using AutoMapper;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Core.Enitities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    // GET: api/Employee
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var employees = await _employeeService.GetEmployeesAsync(pageNumber, pageSize);
        var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        return Ok(employeeDTOs);
    }

    // GET: api/Employee/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
        return Ok(employeeDTO);
    }

    // POST: api/Employee
    [HttpPost]
    public async Task<ActionResult<EmployeeDTO>> PostEmployee(EmployeeDTO employeeDTO)
    {
        await _employeeService.AddEmployeeAsync(employeeDTO);
        return CreatedAtAction(nameof(GetEmployee), new { Message = "New employee created successfully!" });
    }

    // PUT: api/Employee/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employeeDTO)
    {
        if (id != employeeDTO.EmployeeNumber)
        {
            return BadRequest();
        }

        await _employeeService.UpdateEmployeeAsync(employeeDTO);

        return NoContent();
    }

    // DELETE: api/Employee/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await _employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }
}