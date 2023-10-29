using Api.Models.Classes;
using Api.Models.Dtos;
using Api.Models.Enums;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services
{
    public class GameService : IGameService
    {
        private readonly ILogger<GameService> _logger;
        private readonly IDDragonCdnService _dDragonCdnService;

        public GameService(IDDragonCdnService dDragonCdnService, ILogger<GameService> logger)
        {
            _dDragonCdnService = dDragonCdnService;
            _logger = logger;
        }

        public IActionResult GetQuestion(QuestionType questionType)
        {
            ParsedChampion parsedChampion;
            try
            {
                parsedChampion = _dDragonCdnService.GetRandomParsedChampion();
            }
            catch(Exception e)
            {
                _logger.LogError("DDragonCdnService has not receieved data yet.");
                ObjectResult result = new ObjectResult("");
                result.StatusCode = 500;
                return result;
            }
           
            QuestionDto questionDto = new QuestionDto();
            
            switch(questionType)
            {
                case QuestionType.Lore:
                    questionDto.Id = parsedChampion.RedactedLore.Key;
                    questionDto.Type = QuestionType.Lore;
                    questionDto.Value = parsedChampion.RedactedLore.Value;
                    break;
                case QuestionType.Spell:
                    questionDto.Id = parsedChampion.SpellUrls.Key;
                    questionDto.Type = QuestionType.Spell;
                    questionDto.Value = parsedChampion.SpellUrls.Value.ElementAt(Random.Shared.Next(0, parsedChampion.SpellUrls.Value.Count));
                    break;
                case QuestionType.Splash:
                    questionDto.Id = parsedChampion.SplashArtUrls.Key;
                    questionDto.Type = QuestionType.Splash;
                    questionDto.Value = parsedChampion.SplashArtUrls.Value.ElementAt(Random.Shared.Next(0, parsedChampion.SplashArtUrls.Value.Count));
                    break;
                default:
                    return new BadRequestResult();
            }

            return new OkObjectResult(questionDto);
        }
    }
}
