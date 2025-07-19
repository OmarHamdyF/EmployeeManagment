using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Common.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid id);
        Task<IEnumerable<Role>> GetAllAsync();
        Task AddAsync(Role role);
        void Update(Role role);
        void Delete(Role role);
        Task<Role?> GetByNameAsync(string name); // Useful for uniqueness checks
    }

}
