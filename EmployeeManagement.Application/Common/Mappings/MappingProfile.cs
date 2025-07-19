using AutoMapper;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment;
using EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;
using EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;
using EmployeeManagement.Application.Features.Roles.Commands.CreateRole;
using EmployeeManagement.Application.Features.Roles.Commands.UpdateRole;
using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Employee Mappings
            CreateMap<CreateEmployeeCommand, Employee>();
            CreateMap<UpdateEmployeeCommand, Employee>();
            CreateMap<Employee, EmployeeDto>();

            // Department Mappings
            CreateMap<CreateDepartmentCommand, Department>();
            CreateMap<UpdateDepartmentCommand, Department>();
            CreateMap<Department, DepartmentDto>();

            // Role Mappings
            CreateMap<CreateRoleCommand, Role>();
            CreateMap<UpdateRoleCommand, Role>();
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions)); // Map the Dictionary directly
        }
    }

}
