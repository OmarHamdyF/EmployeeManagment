using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Common.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(Guid id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task AddAsync(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
        Task<IEnumerable<Employee>> SearchAsync(
            string? name, string? departmentName, bool? isActive, DateTime? dateOfJoiningFrom, DateTime? dateOfJoiningTo);
        Task<Employee?> GetByEmailAsync(string email); // For unique email validation
    }

}
