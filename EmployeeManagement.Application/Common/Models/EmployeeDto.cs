using EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Common.Models
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public Guid DepartmentId { get; set; }
        public DepartmentDto Department { get; set; } = default!; // Nested DTO
        public Guid RoleId { get; set; }
        public RoleDto Role { get; set; } = default!; // Nested DTO
        public DateTime DateOfJoining { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
