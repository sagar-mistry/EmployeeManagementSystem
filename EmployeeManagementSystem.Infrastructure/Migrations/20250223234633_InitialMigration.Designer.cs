﻿// <auto-generated />
using EmployeeManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmployeeManagementSystem.Infrastructure.Migrations
{
    [DbContext(typeof(EmployeeDbContext))]
    [Migration("20250223234633_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmployeeManagementSystem.Core.Enitities.Employee", b =>
                {
                    b.Property<int>("EmployeeNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EmployeeId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeNumber"));

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("FullName");

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("HourlyPayRate");

                    b.Property<int>("HoursWorked")
                        .HasColumnType("int")
                        .HasColumnName("HoursWorked");

                    b.Property<decimal>("TotalPay")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("TotalPay");

                    b.HasKey("EmployeeNumber");

                    b.ToTable("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
