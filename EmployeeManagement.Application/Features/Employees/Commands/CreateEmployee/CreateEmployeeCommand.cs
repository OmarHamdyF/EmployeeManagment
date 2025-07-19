using EmployeeManagement.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<EmployeeDto> // EmployeeDto defined below
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = default!;

        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; } = default!;

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        public DateTime DateOfJoining { get; set; } = DateTime.UtcNow; // Default to now

        public bool IsActive { get; set; } = true; // Default to active
    }

}
