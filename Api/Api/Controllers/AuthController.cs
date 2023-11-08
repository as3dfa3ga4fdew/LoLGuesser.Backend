using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Enums;
using Api.Models.Schemas;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginSchema schema)
        {
            if (!ModelState.IsValid)
                return BadRequest(new StatusDto(ErrorType.InvalidModel));

            if (!_userService.Validate(schema))
                return BadRequest(new StatusDto(ErrorType.InvalidSchema));

            UserEntity user = await _userService.GetByUsernameAsync(schema.Username);
            if(user == null)
                return Unauthorized(new StatusDto(ErrorType.InvalidCredentials));

            if (!await _userService.VerifyPasswordAsync(schema.Password, user.Password))
                return Unauthorized(new StatusDto(ErrorType.InvalidCredentials));

            LoginDto loginDto = new LoginDto()
            {
                Username = user.Username,
                Score = user.Score,
                Jwt = _jwtService.Create(new List<Claim>() { new Claim("id", user.Id.ToString()) })
            };

            return Ok(loginDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterSchema schema)
        {
            if(!ModelState.IsValid)
                return BadRequest(new StatusDto(ErrorType.InvalidModel));

            if (!_userService.Validate(schema))
                return BadRequest(new StatusDto(ErrorType.InvalidSchema));

            UserEntity user = await _userService.GetByUsernameAsync(schema.Username);
            if (user != null)
                return Conflict(new StatusDto(ErrorType.UsernameAlreadyTaken));

            user = new UserEntity()
            {
                Username = schema.Username,
                Password = await _userService.HashPasswordAsync(schema.Password),
                Score = 0
            };

            if (!await _userService.CreateAsync(user))
                throw new Exception(nameof(_userService.CreateAsync));

            RegisterDto registerDto = new RegisterDto()
            {
                Username = user.Username,
                Score = user.Score,
                Jwt = _jwtService.Create(new List<Claim>() { new Claim("id", user.Id.ToString()) })
            };

            return Created("",registerDto);
        }

        [HttpGet("token/validate")]
        [Authorize]
        public async Task<IActionResult> ValidateTokenAsync()
        {
            return NoContent();
        }
    }
}
