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

namespace EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(request.Id);
            if (employee == null)
            {
                throw new NotFoundException(nameof(Employee), request.Id);
            }

            // Check for duplicate email if email is being changed
            if (employee.Email != request.Email)
            {
                var existingEmployeeWithEmail = await _unitOfWork.Employees.GetByEmailAsync(request.Email);
                if (existingEmployeeWithEmail != null && existingEmployeeWithEmail.Id != request.Id)
                {
                    throw new ValidationException($"Employee with email '{request.Email}' already exists.");
                }
            }

            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (department == null)
            {
                throw new NotFoundException(nameof(Department), request.DepartmentId);
            }

            var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new NotFoundException(nameof(Role), request.RoleId);
            }

            _mapper.Map(request, employee); // Map command to existing entity

            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Re-fetch with includes to get populated Department/Role DTOs
            var updatedEmployee = await _unitOfWork.Employees.GetByIdAsync(employee.Id);
            return _mapper.Map<EmployeeDto>(updatedEmployee);
        }
    }

}
