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

namespace EmployeeManagement.Application.Features.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(request.Id);
            if (role == null)
            {
                throw new NotFoundException(nameof(Role), request.Id);
            }

            // Check for duplicate name if name is being changed
            if (role.Name != request.Name)
            {
                var existingRoleWithName = await _unitOfWork.Roles.GetByNameAsync(request.Name);
                if (existingRoleWithName != null && existingRoleWithName.Id != request.Id)
                {
                    throw new ValidationException($"Role with name '{request.Name}' already exists.");
                }
            }

            _mapper.Map(request, role); // Map command to existing entity

            _unitOfWork.Roles.Update(role);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return _mapper.Map<RoleDto>(role);
        }
    }

}
