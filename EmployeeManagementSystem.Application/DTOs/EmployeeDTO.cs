using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Application.DTOs;


/// <summary>
/// Data Transfer Object (DTO) representing an employee.
/// </summary>
public class EmployeeDTO
{
    /// <summary>
    /// Unique identifier for the employee.
    /// </summary>
    /// <example>12345</example>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeNumber { get; set; }

    /// <summary>
    /// Full name of the employee.
    /// </summary>
    /// <example>John Doe</example>
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string EmployeeName { get; set; }

    /// <summary>
    /// Hourly pay rate of the employee.
    /// </summary>
    /// <example>25.50</example>
    [Required]
    [Range(10, 500)]
    public decimal HourlyRate { get; set; }

    /// <summary>
    /// Number of hours worked by the employee.
    /// </summary>
    /// <example>40</example>
    [Required]
    [Range(1, 168)]
    public int HoursWorked { get; set; }

    /// <summary>
    /// Total pay calculated for the employee (HourlyRate * HoursWorked).
    /// </summary>
    /// <example>1020.00</example>
    public decimal TotalPay { get; set; }
}