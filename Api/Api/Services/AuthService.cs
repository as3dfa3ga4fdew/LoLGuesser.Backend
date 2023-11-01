using Api.Helpers;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Schemas;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Services
{
    public class AuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public AuthService(IJwtService jwtService, IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> LoginAsync(LoginSchema schema)
        {
            if (!Validate.Username(schema.Username))
                return new BadRequestObjectResult(null);

            if(!Validate.Password(schema.Password))
                return new BadRequestObjectResult(null);

            UserEntity user = await _userRepository.GetByUsernameAsync(schema.Username);
            if (user == null)
                return new UnauthorizedObjectResult(null);

            if(!BCrypt.Net.BCrypt.Verify(schema.Password, user.Password))
                return new UnauthorizedObjectResult(null);

            LoginDto loginDto = new LoginDto();
            loginDto.Username = user.Username;
            loginDto.Jwt = _jwtService.Create(new List<Claim>() { new Claim("username", schema.Username) });
            loginDto.Score = loginDto.Score;

            return new OkObjectResult(loginDto);
        }
    }
}
