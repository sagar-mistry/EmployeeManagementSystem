using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Core.Enitities;

[Table("Employees")]
public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("EmployeeId")]
    public int EmployeeNumber { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    [Column("FullName", TypeName = "nvarchar(100)")]
    public string EmployeeName { get; set; }

    [Required]
    [Range(10, 500)]
    [Column("HourlyPayRate", TypeName = "decimal(10,2)")]
    public decimal HourlyRate { get; set; }

    [Required]
    [Range(1, 168)]
    [Column("HoursWorked", TypeName = "int")]
    public int HoursWorked { get; set; }

    [Required]
    [Column("TotalPay", TypeName = "decimal(10,2)")]
    public decimal TotalPay { get; set; }
}