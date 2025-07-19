using EmployeeManagement.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Features.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest<RoleDto>
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        public Dictionary<string, bool> Permissions { get; set; } = new Dictionary<string, bool>();
    }

}
