using EmployeeManagement.Application.Common.Exceptions;
using EmployeeManagement.Application.Features.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _mediator.Send(request);
                return Ok(response);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new LoginResponse { Succeeded = false, Message = ex.Message });
            }
            catch (ValidationException ex) // If you use FluentValidation and MediatR pipeline behaviors
            {
                return BadRequest(new LoginResponse { Succeeded = false, Message = "Validation failed" }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse { Succeeded = false, Message = "An unexpected error occurred during login." });
            }
        }

    }
}
