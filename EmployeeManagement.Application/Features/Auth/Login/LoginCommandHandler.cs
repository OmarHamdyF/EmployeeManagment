using EmployeeManagement.Application.Common.Exceptions;
using EmployeeManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using System.IdentityModel.Tokens.Jwt;

namespace EmployeeManagement.Application.Features.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {

            // Dummy validation for demonstration:
            bool isValidUser = (request.Email == "test@example.com" && request.Password == "Pa$$w0rd");

            if (!isValidUser)
            {
                throw new UnauthorizedException("Invalid login credentials.");
            }
            var userId = Guid.NewGuid().ToString(); // Placeholder
            var userName = "Test User";
            var userRoles = new List<string> { "Viewer", "Admin", "HR" }; 


            // --- 2. Create Claims for the JWT ---
            var claims = new List<Claim>
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, userId),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, request.Email),
                new Claim(ClaimTypes.Name, userName) // Add username claim
            };
            
            // Add roles as claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // --- 3. Get JWT Settings from Configuration ---
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var key = _configuration["JwtSettings:Key"];

            // Convert the key to a SymmetricSecurityKey
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // --- 4. Create the JWT Token ---
            var tokenExpiration = DateTime.Now.AddMinutes(60);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return new LoginResponse
            {
                Token = tokenString,
                Expiration = tokenExpiration,
                UserName = userName,
                Email = request.Email,
                Roles = userRoles,
                Succeeded = true,
                Message = "Login successful"
            };
        }
    }

}
