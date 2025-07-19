using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Features.Employees.Commands.ActivateDeactivateEmployee;
using EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee;
using EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeeManagement.Application.Features.Employees.Queries.GetAllEmployees;
using EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;
using EmployeeManagement.Application.Features.Employees.Queries.SearchEmployees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagment.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "ViewerPolicy")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _mediator.Send(new GetAllEmployeesQuery());
            return Ok(employees);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ViewerPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(Guid id)
        {
            var employee = await _mediator.Send(new GetEmployeeByIdQuery { Id = id });
            return Ok(employee); 
        }

        [HttpPost]
        [Authorize(Policy = "HRPolicy")] 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
            var employeeDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeDto.Id }, employeeDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "HRPolicy")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeCommand command)
        {
            if (id != command.Id)
            {
                throw new ValidationException("Route ID and request body ID do not match.");
            }
            var employeeDto = await _mediator.Send(command);
            return Ok(employeeDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            await _mediator.Send(new DeleteEmployeeCommand { Id = id });
            return NoContent();
        }

        
        [HttpGet("search")]
        [Authorize(Policy = "ViewerPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> SearchEmployees([FromQuery] SearchEmployeesQuery query)
        {
            var employees = await _mediator.Send(query);
            return Ok(employees);
        }

        [HttpPatch("{id}/status")] 
        [Authorize(Policy = "HRPolicy")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<EmployeeDto>> ActivateDeactivateEmployee(Guid id, [FromBody] ActivateDeactivateEmployeeCommand command)
        {
            if (id != command.Id)
            {
                throw new ValidationException("Route ID and request body ID do not match.");
            }
            var employeeDto = await _mediator.Send(command);
            return Ok(employeeDto);
        }
    }
}
