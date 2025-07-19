using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IEmployeeRepository? _employeeRepository;
        private IDepartmentRepository? _departmentRepository;
        private IRoleRepository? _roleRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEmployeeRepository Employees => _employeeRepository ??= new EmployeeRepository(_context);
        public IDepartmentRepository Departments => _departmentRepository ??= new DepartmentRepository(_context);
        public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_context);

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
