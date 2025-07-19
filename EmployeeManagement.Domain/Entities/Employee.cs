using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Domain.Entities
{
    public class Employee : AuditableEntity
    {
        public Guid Id { get; set; } // Primary Key

        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Phone { get; set; } = default!;

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; } = default!; // Navigation property

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!; // Navigation property

        public DateTime DateOfJoining { get; set; }

        public bool IsActive { get; set; }

        public Employee()
        {
            Id = Guid.NewGuid();
            IsActive = true; // Default to active on creation
            DateOfJoining = DateTime.UtcNow; // Default to current date/time
        }
    }

}
