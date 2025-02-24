using AutoMapper;
using EmployeeManagementSystem.API.Controllers;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeManagementSystem.Tests.ControllerTests
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        private Mock<IEmployeeService> _mockEmployeeService;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<EmployeeController>> _mockLogger;
        private EmployeeController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<EmployeeController>>();

            _controller = new EmployeeController(
                _mockEmployeeService.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        // ✅ Test: GetEmployees returns 200 OK
        [Test]
        public async Task GetEmployees_ReturnsOk_WithEmployeeList()
        {
            // Arrange
            var employees = new List<EmployeeDTO>
            {
                new EmployeeDTO { EmployeeNumber = 1, EmployeeName = "John Doe", HourlyRate = 30, HoursWorked = 40 },
                new EmployeeDTO { EmployeeNumber = 2, EmployeeName = "Jane Smith", HourlyRate = 35, HoursWorked = 38 }
            };
            _mockEmployeeService.Setup(s => s.GetEmployeesAsync(1, 50)).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetEmployees();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(employees, okResult.Value);
        }

        // ✅ Test: GetEmployee returns 200 OK
        [Test]
        public async Task GetEmployee_WithValidId_ReturnsOk()
        {
            // Arrange
            var employee = new EmployeeDTO { EmployeeNumber = 1, EmployeeName = "John Doe", HourlyRate = 30, HoursWorked = 40 };
            _mockEmployeeService.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);

            // Act
            var result = await _controller.GetEmployee(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(employee, okResult.Value);
        }

        // ✅ Test: GetEmployee returns 404 NotFound
        [Test]
        public async Task GetEmployee_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockEmployeeService.Setup(s => s.GetEmployeeByIdAsync(99)).ReturnsAsync((EmployeeDTO)null);

            // Act
            var result = await _controller.GetEmployee(99);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        // ✅ Test: SearchEmployees returns 200 OK with results
        [Test]
        public async Task SearchEmployees_WithValidName_ReturnsOk()
        {
            // Arrange
            var employees = new List<EmployeeDTO>
            {
                new EmployeeDTO { EmployeeNumber = 1, EmployeeName = "Alan Turing", HourlyRate = 40, HoursWorked = 42 }
            };
            _mockEmployeeService.Setup(s => s.SearchEmployeesAsync("Alan")).ReturnsAsync(employees);

            // Act
            var result = await _controller.SearchEmployees("Alan");

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(employees, okResult.Value);
        }

        // ✅ Test: SearchEmployees with empty name returns 400 BadRequest
        [Test]
        public async Task SearchEmployees_WithEmptyName_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.SearchEmployees("");

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        // ✅ Test: CreateEmployee returns 201 Created
        [Test]
        public async Task CreateEmployee_ValidInput_ReturnsCreated()
        {
            // Arrange
            var employeeDto = new EmployeeDTO { EmployeeNumber = 3, EmployeeName = "Ada Lovelace", HourlyRate = 45, HoursWorked = 35 };
            _mockEmployeeService.Setup(s => s.AddEmployeeAsync(employeeDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateEmployee(employeeDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        // ✅ Test: UpdateEmployee returns 204 NoContent
        [Test]
        public async Task UpdateEmployee_ValidInput_ReturnsNoContent()
        {
            // Arrange
            var employeeDto = new EmployeeDTO { EmployeeNumber = 1, EmployeeName = "Updated Name", HourlyRate = 50, HoursWorked = 40 };
            _mockEmployeeService.Setup(s => s.UpdateEmployeeAsync(employeeDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateEmployee(1, employeeDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        // ✅ Test: UpdateEmployee with ID mismatch returns 400 BadRequest
        [Test]
        public async Task UpdateEmployee_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var employeeDto = new EmployeeDTO { EmployeeNumber = 2, EmployeeName = "Mismatch", HourlyRate = 45, HoursWorked = 35 };

            // Act
            var result = await _controller.UpdateEmployee(1, employeeDto);

            // Assert
            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        // ✅ Test: DeleteEmployee returns 204 NoContent
        [Test]
        public async Task DeleteEmployee_ValidId_ReturnsNoContent()
        {
            // Arrange
            _mockEmployeeService.Setup(s => s.DeleteEmployeeAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteEmployee(1);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}

