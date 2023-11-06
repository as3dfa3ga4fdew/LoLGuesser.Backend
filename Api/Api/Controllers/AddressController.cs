
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Enums;
using Api.Models.Schemas;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IJwtService _jwtService;

        public AddressController(IAddressService addressService, IJwtService jwtService)
        {
            _addressService = addressService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(AddressSchema schema)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorDto(ErrorType.InvalidModel));

            if (!_jwtService.TryGetClaim(HttpContext, "id", out Claim claim))
                return BadRequest(new ErrorDto(ErrorType.MissingIdClaim));

            if (!Guid.TryParse(claim.Value, out Guid userId))
                return BadRequest(new ErrorDto(ErrorType.InvalidGuid));

            if (!_addressService.Validate(schema))
                return BadRequest(new ErrorDto(ErrorType.InvalidSchema));

            AddressEntity entity = _addressService.CreateEntity(schema, userId);

            if (!await _addressService.CreateAsync(entity))
                throw new Exception(nameof(CreateAsync));

            return Created("", (AddressDto)entity);
        }
        [HttpGet("/api/[controller]s")]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            if (!_jwtService.TryGetClaim(HttpContext, "id", out Claim claim))
                return BadRequest(new ErrorDto(ErrorType.MissingIdClaim));

            if (!Guid.TryParse(claim.Value, out Guid userId))
                return BadRequest(new ErrorDto(ErrorType.InvalidGuid));

            IEnumerable<AddressDto> addressDtos = (await _addressService.GetAllAsync(userId)).Select(x => (AddressDto)x);

            return Ok(addressDtos);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorDto(ErrorType.InvalidModel));

            if (!_jwtService.TryGetClaim(HttpContext, "id", out Claim claim))
                return BadRequest(new ErrorDto(ErrorType.MissingIdClaim));

            if (!Guid.TryParse(claim.Value, out Guid userId))
                return BadRequest(new ErrorDto(ErrorType.InvalidGuid));

            if(!await _addressService.RemoveAsync(id, userId))
                throw new Exception(nameof(DeleteAsync));

            return NoContent();
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAsync(Guid id, AddressSchema schema)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorDto(ErrorType.InvalidModel));

            if (!_jwtService.TryGetClaim(HttpContext, "id", out Claim claim))
                return BadRequest(new ErrorDto(ErrorType.MissingIdClaim));

            if (!Guid.TryParse(claim.Value, out Guid userId))
                return BadRequest(new ErrorDto(ErrorType.InvalidGuid));

            if (!_addressService.Validate(schema))
                return BadRequest(new ErrorDto(ErrorType.InvalidSchema));

            AddressEntity entity = _addressService.CreateEntity(schema, userId);

            if (!await _addressService.UpdateAsync(entity))
                throw new Exception(nameof(PutAsync));

            return Ok();
        }
    }
}
