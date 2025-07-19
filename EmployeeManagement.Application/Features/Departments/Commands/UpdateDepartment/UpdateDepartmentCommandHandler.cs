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

namespace EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DepartmentDto> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(request.Id);
            if (department == null)
            {
                throw new NotFoundException(nameof(Department), request.Id);
            }

            // Check for duplicate name if name is being changed
            if (department.Name != request.Name)
            {
                var existingDepartmentWithName = await _unitOfWork.Departments.GetByNameAsync(request.Name);
                if (existingDepartmentWithName != null && existingDepartmentWithName.Id != request.Id)
                {
                    throw new ValidationException($"Department with name '{request.Name}' already exists.");
                }
            }

            _mapper.Map(request, department); // Map command to existing entity

            _unitOfWork.Departments.Update(department);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return _mapper.Map<DepartmentDto>(department);
        }
    }

}
