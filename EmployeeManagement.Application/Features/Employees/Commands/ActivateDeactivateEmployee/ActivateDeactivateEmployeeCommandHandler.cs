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

namespace EmployeeManagement.Application.Features.Employees.Commands.ActivateDeactivateEmployee
{
    public class ActivateDeactivateEmployeeCommandHandler : IRequestHandler<ActivateDeactivateEmployeeCommand, EmployeeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivateDeactivateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(ActivateDeactivateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(request.Id);
            if (employee == null)
            {
                throw new NotFoundException(nameof(Employee), request.Id);
            }

            employee.IsActive = request.IsActive; // Update status
            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var updatedEmployee = await _unitOfWork.Employees.GetByIdAsync(employee.Id);
            return _mapper.Map<EmployeeDto>(updatedEmployee);
        }
    }

}
