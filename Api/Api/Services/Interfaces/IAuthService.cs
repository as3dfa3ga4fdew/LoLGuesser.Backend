using Api.Models.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<IActionResult> LoginAsync(LoginSchema schema);
        public Task<IActionResult> RegisterAsync(RegisterSchema schema);
    }
}
