using EmployeeManagementSystem.Core.Enitities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.Data;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

    protected EmployeeDbContext()
    {
    }

    public DbSet<Employee> Employees { get; set; }
}