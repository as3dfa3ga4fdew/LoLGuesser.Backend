using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Enums;
using Api.Models.Schemas;
using Api.Repositories.Interfaces;
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
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public GameController(IGameService gameService, IUserRepository userRepository, IJwtService jwtService)
        {
            _gameService = gameService;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetChampionNamesAsync()
        {
            return _gameService.GetChampionNames();
        }

        [HttpPost("question")]
        public async Task<IActionResult> GetQeuestionAsync(QuestionSchema questionSchema)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return _gameService.GetQuestion(questionSchema.Type);
        }

        [HttpPost("answer/guest")]
        public async Task<IActionResult> VerifyGuestAnswerAsync(AnswerSchema schema)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return _gameService.VerifyAnswer(schema);
        }

        [HttpPost("answer")]
        [Authorize]
        public async Task<IActionResult> VerifyAnswerAsync(AnswerSchema schema)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if(!_jwtService.TryGetClaim(HttpContext, "username", out Claim claim))
                return BadRequest(new ErrorDto(ErrorType.MissingIdClaim));

            IActionResult result = _gameService.VerifyAnswer(schema);

            OkObjectResult okObjectResult = result as OkObjectResult;
            if (okObjectResult == null)
                return result;

            AnswerDto answer = okObjectResult.Value as AnswerDto;

            if (!answer.Correct)
                return result;

            UserEntity user = await _userRepository.GetByUsernameAsync(claim.Value);
            if (user == null)
                return Unauthorized();

            user.Score++;

            if (!await _userRepository.UpdateAsync(user))
                return StatusCode(500);

            answer.Score = user.Score;

            return result;
        }
    }
}
