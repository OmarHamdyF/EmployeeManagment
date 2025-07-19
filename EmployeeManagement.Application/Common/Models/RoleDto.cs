using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Common.Models
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public Dictionary<string, bool> Permissions { get; set; } = new Dictionary<string, bool>();
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
