using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Domain.Entities
{
    public class Department : AuditableEntity
    {
        public Guid Id { get; set; } // Primary Key

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public Department()
        {
            Id = Guid.NewGuid();
        }
    }
}
