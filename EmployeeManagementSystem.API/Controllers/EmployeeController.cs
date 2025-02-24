using AutoMapper;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Core.Enitities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementSystem.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeController> _logger;
    public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _mapper = mapper;
        _logger = logger;
    }

    
    // GET: api/Employee/GetEmployees?pageNumber=1&pageSize=50
    /// <summary>
    /// Get list of all the empoyees by page number, and page size
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns>Returns the list of by page number and page size.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        var employees = await _employeeService.GetEmployeesAsync(pageNumber, pageSize);
        return Ok(employees);
    }


    // GET: api/Employee/GetEmployee/11
    /// <summary>
    /// Get an employee by their EmployeeID.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <returns>The employee, or NotFound if not found.</returns>
    /// <response code="200">Returns the employee.</response>
    /// <response code="404">If the employee is not found.</response>
    /// <response code="500">If any exception occurs.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            var activityId = HttpContext?.Items["ActivityId"] ?? string.Empty;
            _logger.LogWarning("Employee with ID: {Id} not found. ActivityId: {ActivityId}", id, activityId);
            return NotFound();
        }
        return Ok(employee);
    }


    /// <summary>
    /// Search employees by name, case-insensitive
    /// </summary>
    /// <param name="name"></param>
    /// <returns>List of employees</returns>
    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> SearchEmployees([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            var activityId = HttpContext?.Items["ActivityId"] ?? string.Empty;
            _logger.LogWarning("SearchEmployees request failed. ActivityId: {ActivityId}, Empty search term.", activityId);
            return BadRequest("Search term cannot be empty.");
        }

        var employees = await _employeeService.SearchEmployeesAsync(name);
        return Ok(employees);
    }



    // POST: api/Employee/PostEmployee
    /// <summary>
    /// Creates new Employee record.
    /// </summary>
    /// <param name="employeeDTO"></param>
    /// <returns>201 creted code.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EmployeeDTO>> CreateEmployee(EmployeeDTO employeeDTO)
    {
        await _employeeService.AddEmployeeAsync(employeeDTO);
        return CreatedAtAction(nameof(CreateEmployee), new { Message = "New employee created successfully!" });
    }


    // PUT: api/Employee/PutEmployee/1
    /// <summary>
    /// Updates the existing employee data.
    /// </summary>
    /// <param name="id">Employee Id/number</param>
    /// <param name="employeeDTO">Employee object with data</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeDTO employeeDTO)
    {
        if (id != employeeDTO.EmployeeNumber)
        {
            var activityId = HttpContext?.Items["ActivityId"] ?? string.Empty;
            _logger.LogWarning("UpdateEmployee request failed. ActivityId: {ActivityId}, ID mismatch. Expected: {ExpectedId}, Received: {ReceivedId}", activityId, id, employeeDTO.EmployeeNumber);
            return BadRequest();
        }

        await _employeeService.UpdateEmployeeAsync(employeeDTO);
        return NoContent();
    }

    // DELETE: api/Employee/DeleteEmployee/2
    /// <summary>
    /// Deletes the employee by employeeId
    /// </summary>
    /// <param name="id">Employee id/number</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEmployee(int id)
    {

        await _employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }
}

