using EmployeeManagement.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Features.Employees.Commands.ActivateDeactivateEmployee
{
    public class ActivateDeactivateEmployeeCommand : IRequest<EmployeeDto>
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } // The desired active status
    }

}
