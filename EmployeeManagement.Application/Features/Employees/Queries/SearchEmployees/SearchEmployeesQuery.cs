using EmployeeManagement.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Features.Employees.Queries.SearchEmployees
{
    public class SearchEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
    {
        public string? Name { get; set; }
        public string? DepartmentName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateOfJoiningFrom { get; set; }
        public DateTime? DateOfJoiningTo { get; set; }
    }

}
