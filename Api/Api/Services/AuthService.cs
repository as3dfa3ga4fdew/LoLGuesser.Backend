using Api.Helpers;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Schemas;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
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
            if (schema == null) throw new ArgumentNullException(nameof(LoginSchema));

            if (!Validate.Username(schema.Username))
                return new BadRequestResult();

            if(!Validate.Password(schema.Password))
                return new BadRequestResult();

            UserEntity user = await _userRepository.GetByUsernameAsync(schema.Username);
            if (user == null)
                return new UnauthorizedResult();
            
            if (!await VerifyPasswordAsync(schema.Password, user.Password))
                return new UnauthorizedResult();

            LoginDto loginDto = new LoginDto();
            loginDto.Username = user.Username;
            loginDto.Jwt = _jwtService.Create(new List<Claim>() { new Claim("username", schema.Username) });
            loginDto.Score = loginDto.Score;

            return new OkObjectResult(loginDto);
        }


        public async Task<IActionResult> RegisterAsync(RegisterSchema schema)
        {
            if (schema == null) throw new ArgumentNullException(nameof(LoginSchema));

            if (!Validate.Username(schema.Username))
                return new BadRequestResult();

            if (!Validate.Password(schema.Password))
                return new BadRequestResult();

            if (await _userRepository.GetByUsernameAsync(schema.Username) != null)
                return new ConflictResult();

            UserEntity userEntity = new UserEntity()
            { 
                Username = schema.Username,
                Password = await HashPasswordAsync(schema.Password),
                Score = 0
            };

            if(!await _userRepository.CreateAsync(userEntity))
            {
                ObjectResult result = new ObjectResult("");
                result.StatusCode = 500;
                return result;
            }

            RegisterDto registerDto = new RegisterDto()
            {
                Jwt = _jwtService.Create(new List<Claim>() { new Claim("username", userEntity.Username) }),
                Username = userEntity.Username,
                Score = userEntity.Score
            };

            return new OkObjectResult(registerDto);
        }

        [ExcludeFromCodeCoverage]
        public async Task<bool> VerifyPasswordAsync(string plain, string hash)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(plain, hash));
        }
        [ExcludeFromCodeCoverage]
        public async Task<string> HashPasswordAsync(string plain, int costFactor = 10)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(plain, costFactor));
        }

    }
}
