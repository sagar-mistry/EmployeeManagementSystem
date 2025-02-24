using AutoMapper;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Services;
using EmployeeManagementSystem.Core.Enitities;
using EmployeeManagementSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeManagementSystem.Tests.ServiceTests
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<EmployeeService>> _mockLogger;
        private EmployeeService _employeeService;

        [SetUp]
        public void SetUp()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<EmployeeService>>();

            _employeeService = new EmployeeService(
                _mockEmployeeRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        // Test: GetEmployeesAsync returns employees
        [Test]
        public async Task GetEmployeesAsync_ReturnsMappedEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { EmployeeNumber = 1, EmployeeName = "John Doe", HourlyRate = 30, HoursWorked = 40 },
                new Employee { EmployeeNumber = 2, EmployeeName = "Jane Smith", HourlyRate = 35, HoursWorked = 38 }
            };
            var employeeDTOs = employees.Select(e => new EmployeeDTO { EmployeeNumber = e.EmployeeNumber, EmployeeName = e.EmployeeName, HourlyRate = e.HourlyRate, HoursWorked = e.HoursWorked });

            _mockEmployeeRepository.Setup(r => r.GetEmployeesAsync(1, 50)).ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<IEnumerable<EmployeeDTO>>(employees)).Returns(employeeDTOs);

            // Act
            var result = await _employeeService.GetEmployeesAsync(1, 50);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("John Doe", result.First().EmployeeName);
        }

        // Test: GetEmployeeByIdAsync returns employee
        [Test]
        public async Task GetEmployeeByIdAsync_WithValidId_ReturnsEmployeeDTO()
        {
            // Arrange
            var employee = new Employee { EmployeeNumber = 1, EmployeeName = "John Doe", HourlyRate = 30, HoursWorked = 40 };
            var employeeDTO = new EmployeeDTO { EmployeeNumber = 1, EmployeeName = "John Doe", HourlyRate = 30, HoursWorked = 40 };

            _mockEmployeeRepository.Setup(r => r.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
            _mockMapper.Setup(m => m.Map<EmployeeDTO>(employee)).Returns(employeeDTO);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("John Doe", result.EmployeeName);
        }

        // Test: GetEmployeeByIdAsync returns null if employee doesn't exist
        [Test]
        public async Task GetEmployeeByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockEmployeeRepository.Setup(r => r.GetEmployeeByIdAsync(99)).ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(99);

            // Assert
            Assert.IsNull(result);
        }

        // Test: AddEmployeeAsync calls repository
        [Test]
        public async Task AddEmployeeAsync_ValidEmployee_CallsRepository()
        {
            // Arrange
            var employeeDTO = new EmployeeDTO { EmployeeNumber = 1, EmployeeName = "New Employee", HourlyRate = 30, HoursWorked = 40 };
            var employee = new Employee { EmployeeNumber = 1, EmployeeName = "New Employee", HourlyRate = 30, HoursWorked = 40 };

            _mockMapper.Setup(m => m.Map<Employee>(employeeDTO)).Returns(employee);
            _mockEmployeeRepository.Setup(r => r.AddEmployeeAsync(employee)).Returns(Task.CompletedTask);

            // Act
            await _employeeService.AddEmployeeAsync(employeeDTO);

            // Assert
            _mockEmployeeRepository.Verify(r => r.AddEmployeeAsync(employee), Times.Once);
        }

        // Test: UpdateEmployeeAsync calls repository
        [Test]
        public async Task UpdateEmployeeAsync_ValidEmployee_CallsRepository()
        {
            // Arrange
            var employeeDTO = new EmployeeDTO { EmployeeNumber = 1, EmployeeName = "Updated Name", HourlyRate = 50, HoursWorked = 40 };
            var employee = new Employee { EmployeeNumber = 1, EmployeeName = "Updated Name", HourlyRate = 50, HoursWorked = 40 };

            _mockMapper.Setup(m => m.Map<Employee>(employeeDTO)).Returns(employee);
            _mockEmployeeRepository.Setup(r => r.UpdateEmployeeAsync(employee)).Returns(Task.CompletedTask);

            // Act
            await _employeeService.UpdateEmployeeAsync(employeeDTO);

            // Assert
            _mockEmployeeRepository.Verify(r => r.UpdateEmployeeAsync(employee), Times.Once);
        }

        // Test: DeleteEmployeeAsync calls repository
        [Test]
        public async Task DeleteEmployeeAsync_WithValidId_CallsRepository()
        {
            // Arrange
            _mockEmployeeRepository.Setup(r => r.DeleteEmployeeAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _employeeService.DeleteEmployeeAsync(1);

            // Assert
            _mockEmployeeRepository.Verify(r => r.DeleteEmployeeAsync(1), Times.Once);
        }

        // Test: SearchEmployeesAsync returns employees
        [Test]
        public async Task SearchEmployeesAsync_WithValidName_ReturnsResults()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { EmployeeNumber = 1, EmployeeName = "Alan Turing", HourlyRate = 40, HoursWorked = 42 }
            };
            var employeeDTOs = employees.Select(e => new EmployeeDTO { EmployeeNumber = e.EmployeeNumber, EmployeeName = e.EmployeeName, HourlyRate = e.HourlyRate, HoursWorked = e.HoursWorked });

            _mockEmployeeRepository.Setup(r => r.SearchEmployeesAsync("Alan")).ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<IEnumerable<EmployeeDTO>>(employees)).Returns(employeeDTOs);

            // Act
            var result = await _employeeService.SearchEmployeesAsync("Alan");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Alan Turing", result.First().EmployeeName);
        }
    }
}
