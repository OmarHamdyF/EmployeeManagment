using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Features.Employees.Queries.SearchEmployees
{
    public class SearchEmployeesQueryHandler : IRequestHandler<SearchEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(SearchEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.Employees.SearchAsync(
                request.Name,
                request.DepartmentName,
                request.IsActive,
                request.DateOfJoiningFrom,
                request.DateOfJoiningTo);

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
    }

}
