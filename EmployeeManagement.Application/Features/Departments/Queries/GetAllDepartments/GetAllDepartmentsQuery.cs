using EmployeeManagement.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Features.Departments.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQuery : IRequest<IEnumerable<DepartmentDto>>
    {
    }

}
