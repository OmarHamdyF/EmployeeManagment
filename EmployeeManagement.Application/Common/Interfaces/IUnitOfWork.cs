using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }
        IDepartmentRepository Departments { get; }
        IRoleRepository Roles { get; }
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }

}
