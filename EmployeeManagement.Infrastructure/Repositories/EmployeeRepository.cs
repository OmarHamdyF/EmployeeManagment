using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Department) // Eager load related entities
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public void Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Employee>> SearchAsync(
            string? name, string? departmentName, bool? isActive, DateTime? dateOfJoiningFrom, DateTime? dateOfJoiningTo)
        {
            IQueryable<Employee> query = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role);

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                query = query.Where(e => e.Department != null && e.Department.Name.Contains(departmentName));
            }

            if (isActive.HasValue)
            {
                query = query.Where(e => e.IsActive == isActive.Value);
            }

            if (dateOfJoiningFrom.HasValue)
            {
                query = query.Where(e => e.DateOfJoining >= dateOfJoiningFrom.Value);
            }

            if (dateOfJoiningTo.HasValue)
            {
                // Ensure the comparison is until the end of the day
                query = query.Where(e => e.DateOfJoining <= dateOfJoiningTo.Value.Date.AddDays(1).AddTicks(-1));
            }

            return await query.ToListAsync();
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }
    }

}
