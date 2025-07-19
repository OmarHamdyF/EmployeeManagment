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

namespace EmployeeManagement.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var existingRole = await _unitOfWork.Roles.GetByNameAsync(request.Name);
            if (existingRole != null)
            {
                throw new ValidationException($"Role with name '{request.Name}' already exists.");
            }

            var role = _mapper.Map<Role>(request);

            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return _mapper.Map<RoleDto>(role);
        }
    }

}
