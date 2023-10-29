using Api.Models.Schemas;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("names")]
        public IActionResult GetChampionNamesAsync()
        {
            return _gameService.GetChampionNames();
        }

        [HttpPost("question")]
        public IActionResult GetQeuestionAsync(QuestionSchema questionSchema)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return _gameService.GetQuestion(questionSchema.Type);
        }
    }
}
