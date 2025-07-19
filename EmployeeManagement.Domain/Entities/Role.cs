using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Domain.Entities
{
    public class Role : AuditableEntity
    {
        public Guid Id { get; set; } // Primary Key
        public string Name { get; set; } = default!;
        public string PermissionsJson { get; set; } = "{}"; // Default to empty JSON object
        // This will be mapped by EF Core to/from PermissionsJson
        [System.ComponentModel.DataAnnotations.Schema.NotMapped] 
        public Dictionary<string, bool> Permissions
        {
            get => string.IsNullOrEmpty(PermissionsJson) ? new Dictionary<string, bool>() : JsonConvert.DeserializeObject<Dictionary<string, bool>>(PermissionsJson) ?? new Dictionary<string, bool>();
            set => PermissionsJson = JsonConvert.SerializeObject(value);
        }

        // Navigation property for Employees having this role
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public Role()
        {
            Id = Guid.NewGuid();
        }
    }

    public enum PermissionType
    {
        ViewEmployees,
        ManageEmployees,
        ViewDepartments,
        ManageDepartments,
        ViewRoles,
        ManageRoles,
        ActivateDeactivateEmployees,
    }

}
