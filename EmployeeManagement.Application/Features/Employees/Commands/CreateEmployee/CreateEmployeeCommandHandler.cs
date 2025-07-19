using AutoMapper;
using EmployeeManagement.Application.Common.Exceptions;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicate email
            var existingEmployee = await _unitOfWork.Employees.GetByEmailAsync(request.Email);
            if (existingEmployee != null)
            {
                throw new ValidationException($"An employee with email '{request.Email}' already exists.");
            }

            // Check that Department exists
            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (department == null)
            {
                throw new NotFoundException(nameof(Department), request.DepartmentId);
            }

            // Check that Role exists
            var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new NotFoundException(nameof(Role), request.RoleId);
            }

            var employee = _mapper.Map<Employee>(request);

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return _mapper.Map<EmployeeDto>(employee);
        }
    }

}
