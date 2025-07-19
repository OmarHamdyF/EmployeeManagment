using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Common.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByIdAsync(Guid id);
        Task<IEnumerable<Department>> GetAllAsync();
        Task AddAsync(Department department);
        void Update(Department department);
        void Delete(Department department);
        Task<Department?> GetByNameAsync(string name); // Useful for uniqueness checks
    }

}
