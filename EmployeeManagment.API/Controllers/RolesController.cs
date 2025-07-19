using EmployeeManagement.Application.Common.Exceptions;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Features.Roles.Commands.CreateRole;
using EmployeeManagement.Application.Features.Roles.Commands.DeleteRole;
using EmployeeManagement.Application.Features.Roles.Commands.UpdateRole;
using EmployeeManagement.Application.Features.Roles.Queries.GetAllRoles;
using EmployeeManagement.Application.Features.Roles.Queries.GetRoleById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagment.API.Controllers
{
    [Authorize] // All endpoints in this controller require authentication by default
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "ViewerPolicy")] // Specific authorization: Viewer, HR, or Admin can view roles
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Returned if authentication fails
        [ProducesResponseType(StatusCodes.Status403Forbidden)]    // Returned if user is authenticated but not authorized
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());
            return Ok(roles);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ViewerPolicy")] // Specific authorization: Viewer, HR, or Admin can view a specific role
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Returned if the role is not found (handled by middleware)
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
        {
            // The NotFoundException thrown by the handler will be caught by GlobalExceptionHandlingMiddleware
            // and translated into a 404 Not Found response.
            var role = await _mediator.Send(new GetRoleByIdQuery { Id = id });
            return Ok(role);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")] // Only Admin can create roles
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Returned for validation errors (handled by middleware)
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleCommand command)
        {
            var roleDto = await _mediator.Send(command);
            // Returns a 201 Created status code with the location of the newly created resource
            return CreatedAtAction(nameof(GetRoleById), new { id = roleDto.Id }, roleDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")] // Only Admin can update roles
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Returned for validation errors or ID mismatch
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Returned if the role is not found
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RoleDto>> UpdateRole(Guid id, [FromBody] UpdateRoleCommand command)
        {
            // Ensure the ID in the route matches the ID in the command body for consistency and security
            if (id != command.Id)
            {
                throw new ValidationException("Route ID and request body ID do not match.");
            }
            var roleDto = await _mediator.Send(command);
            return Ok(roleDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")] // Only Admin can delete roles
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Returned on successful deletion
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Returned if the role is not found
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Returned if trying to delete a role with assigned employees (if explicitly handled by handler)
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteRole(Guid id)
        {
            await _mediator.Send(new DeleteRoleCommand { Id = id });
            return NoContent(); 
        }
    }
}
