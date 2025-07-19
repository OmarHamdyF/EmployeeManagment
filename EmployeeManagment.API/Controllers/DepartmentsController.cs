using EmployeeManagement.Application.Common.Exceptions;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment;
using EmployeeManagement.Application.Features.Departments.Commands.DeleteDepartment;
using EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;
using EmployeeManagement.Application.Features.Departments.Queries.GetAllDepartments;
using EmployeeManagement.Application.Features.Departments.Queries.GetDepartmentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagment.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "ViewerPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
        {
            var departments = await _mediator.Send(new GetAllDepartmentsQuery());
            return Ok(departments);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ViewerPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(Guid id)
        {
            var department = await _mediator.Send(new GetDepartmentByIdQuery { Id = id });
            return Ok(department);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")] // Only Admin can create departments
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentCommand command)
        {
            var departmentDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = departmentDto.Id }, departmentDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")] // Only Admin can update departments
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<DepartmentDto>> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentCommand command)
        {
            if (id != command.Id)
            {
                throw new ValidationException("Route ID and request body ID do not match.");
            }
            var departmentDto = await _mediator.Send(command);
            return Ok(departmentDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")] // Only Admin can delete departments
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // If trying to delete department with employees
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteDepartment(Guid id)
        {
            await _mediator.Send(new DeleteDepartmentCommand { Id = id });
            return NoContent();
        }
    }
}
