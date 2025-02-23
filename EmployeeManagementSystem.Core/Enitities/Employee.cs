namespace EmployeeManagementSystem.Core.Enitities;

public class Employee
{
    public int EmployeeNumber { get; set; }
    public string EmployeeName { get; set; }
    public decimal HourlyRate { get; set; }
    public int HoursWorked { get; set; }
    public decimal TotalPay => HourlyRate * HoursWorked;
}