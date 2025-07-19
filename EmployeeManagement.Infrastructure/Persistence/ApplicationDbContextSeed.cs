using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed Departments
            if (!await context.Departments.AnyAsync())
            {
                var hrDept = new Department { Id = Guid.NewGuid(), Name = "Human Resources", Description = "Manages HR activities.", CreatedBy = "System", CreatedAt = DateTime.UtcNow };
                var itDept = new Department { Id = Guid.NewGuid(), Name = "Information Technology", Description = "Manages IT infrastructure.", CreatedBy = "System", CreatedAt = DateTime.UtcNow };
                var salesDept = new Department { Id = Guid.NewGuid(), Name = "Sales", Description = "Handles sales operations.", CreatedBy = "System", CreatedAt = DateTime.UtcNow };

                await context.Departments.AddRangeAsync(hrDept, itDept, salesDept);
                await context.SaveChangesAsync();
            }

            // Seed Roles
            if (!await context.Roles.AnyAsync())
            {
                var adminRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Permissions = new Dictionary<string, bool>
                    {
                        {"CanViewEmployees", true}, {"CanManageEmployees", true},
                        {"CanViewDepartments", true}, {"CanManageDepartments", true},
                        {"CanViewRoles", true}, {"CanManageRoles", true},
                        {"CanActivateDeactivateEmployees", true}
                    },
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow
                };

                var hrRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "HR",
                    Permissions = new Dictionary<string, bool>
                    {
                        {"CanViewEmployees", true}, {"CanManageEmployees", true},
                        {"CanViewDepartments", true},
                        {"CanActivateDeactivateEmployees", true}
                    },
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow
                };

                var viewerRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Viewer",
                    Permissions = new Dictionary<string, bool>
                    {
                        {"CanViewEmployees", true}, {"CanViewDepartments", true}, {"CanViewRoles", true}
                    },
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow
                };

                await context.Roles.AddRangeAsync(adminRole, hrRole, viewerRole);
                await context.SaveChangesAsync();
            }

            // Seed Admin User (This is typically done with ASP.NET Core Identity)
            // For now, let's just seed an example Admin Employee if you don't implement Identity yet
            if (!await context.Employees.AnyAsync(e => e.Email == "admin@example.com"))
            {
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                var itDept = await context.Departments.FirstOrDefaultAsync(d => d.Name == "Information Technology");

                if (adminRole != null && itDept != null)
                {
                    var adminEmployee = new Employee
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin User",
                        Email = "admin@example.com",
                        Phone = "1234567890",
                        DepartmentId = itDept.Id,
                        RoleId = adminRole.Id,
                        DateOfJoining = DateTime.UtcNow.AddYears(-1),
                        IsActive = true,
                        CreatedBy = "System",
                        CreatedAt = DateTime.UtcNow
                    };
                    await context.Employees.AddAsync(adminEmployee);
                    await context.SaveChangesAsync();
                }
            }
        }
    }

}
