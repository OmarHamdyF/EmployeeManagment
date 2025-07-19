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

namespace EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var existingDepartment = await _unitOfWork.Departments.GetByNameAsync(request.Name);
            if (existingDepartment != null)
            {
                throw new ValidationException($"Department with name '{request.Name}' already exists.");
            }

            var department = _mapper.Map<Department>(request);

            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return _mapper.Map<DepartmentDto>(department);
        }
    }

}
