using Api.Models.Classes;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Enums;
using Api.Models.Schemas;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace Api.Services
{
    public class GameService : IGameService
    {
        private readonly ILogger<IGameService> _logger;
        private readonly IDDragonCdnService _dDragonCdnService;

        public GameService(IDDragonCdnService dDragonCdnService, ILogger<IGameService> logger)
        {
            _dDragonCdnService = dDragonCdnService;
            _logger = logger;
        }

        public IActionResult GetChampionNames()
        {
            IImmutableList<string> names = null;
            try
            {
                names = _dDragonCdnService.GetChampionNames();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "DDragonCdnService has not receieved data yet.");
                ObjectResult result = new ObjectResult("");
                result.StatusCode = 500;
                return result;
            }

            return new OkObjectResult(names);
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
                _logger.LogError(e, "DDragonCdnService has not receieved data yet.");
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

        public IActionResult VerifyAnswer(AnswerSchema schema)
        {
            ParsedChampion parsedChampion = null;
            try
            {
                switch (schema.Type)
                {
                    case QuestionType.Lore:
                        _dDragonCdnService.GetParsedChampionByLoreId(schema.Id, out parsedChampion);
                        break;
                    case QuestionType.Spell:
                        _dDragonCdnService.GetParsedChampionBySpellId(schema.Id, out parsedChampion);
                        break;
                    case QuestionType.Splash:
                        _dDragonCdnService.GetParsedChampionBySplashId(schema.Id, out parsedChampion);
                        break;
                    default:
                        return new BadRequestObjectResult(new ErrorDto(ErrorType.InvalidId));
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "DDragonCdnService has not receieved data yet.");
                ObjectResult result = new ObjectResult("");
                result.StatusCode = 500;
                return result;
            }
           
            if (parsedChampion == null)
                return new BadRequestObjectResult(new ErrorDto(ErrorType.InvalidId));

            AnswerDto answerDto = new AnswerDto();
            answerDto.Correct = parsedChampion.Name == schema.Answer;

            return new OkObjectResult(answerDto);
        }
    }
}
