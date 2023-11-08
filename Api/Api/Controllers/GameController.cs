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
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        public GameController(IGameService gameService, IJwtService jwtService, IUserService userService)
        {
            _gameService = gameService;
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetChampionNamesAsync()
        {
            return Ok(_gameService.GetChampionNames());
        }

        [HttpPost("question")]
        public async Task<IActionResult> GetQeuestionAsync(QuestionSchema schema)
        {
            if(!ModelState.IsValid)
                return BadRequest(new StatusDto(ErrorType.InvalidModel));

            if(!_gameService.Validate(schema))
                return BadRequest(new StatusDto(ErrorType.InvalidSchema));

            return Ok(_gameService.GetQuestion(schema.Type));
        }

        [HttpPost("answer")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAnswerAsync(AnswerSchema schema)
        {
            if (!ModelState.IsValid)
                return BadRequest(new StatusDto(ErrorType.InvalidModel));

            if (!_gameService.Validate(schema))
                return BadRequest(new StatusDto(ErrorType.InvalidSchema));

            AnswerDto answerDto = new AnswerDto() { Correct = false };

            if (!_gameService.VerifyAnswer(schema))
                return Ok(answerDto);

            answerDto.Correct = true;

            //Check if authed
            if (!User.Identity.IsAuthenticated)
                return Ok(answerDto);

            //Parse token
            if (!_jwtService.TryGetClaim(HttpContext, "id", out Claim claim))
                return BadRequest(new ErrorDto(ErrorType.MissingIdClaim));

            if (!Guid.TryParse(claim.Value, out Guid userId))
                return BadRequest(new ErrorDto(ErrorType.InvalidGuid));

            //Get user
            UserEntity userEntity = await _userService.GetByIdAsync(userId);
            if (userEntity == null)
                throw new Exception(nameof(VerifyAnswerAsync) + " " + userId);

            userEntity.Score++;
            
            //Update user
            if (!await _userService.UpdateAsync(userEntity))
                throw new Exception(nameof(_userService.UpdateAsync));

            answerDto.Score = userEntity.Score;

            return Ok(answerDto);
        }
    }
}
