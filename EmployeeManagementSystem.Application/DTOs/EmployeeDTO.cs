namespace EmployeeManagementSystem.Application.DTOs;

public class EmployeeDTO
{
    public int EmployeeNumber { get; set; }
    public string EmployeeName { get; set; }
    public decimal HourlyRate { get; set; }
    public int HoursWorked { get; set; }
    public decimal TotalPay { get; set; }
}