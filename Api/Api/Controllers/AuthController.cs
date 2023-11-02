using Api.Models.Schemas;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginSchema schema)
        {
            if (!ModelState.IsValid)
                return new BadRequestResult();

            return await _authService.LoginAsync(schema);
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterSchema schema)
        {
            if(!ModelState.IsValid)
                return new BadRequestResult();

            return await _authService.RegisterAsync(schema);
        }
    }
}
