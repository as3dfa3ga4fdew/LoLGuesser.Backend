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
using System.ComponentModel;

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

        public IEnumerable<string> GetChampionNames()
        {
            return _dDragonCdnService.GetChampionNames();
        }

        public bool Validate<TSchema>(TSchema schema)
        {
            if (schema == null)
                return false;

            switch (schema)
            {
                case QuestionSchema questionSchema:
                    if (!Enum.IsDefined(typeof(QuestionType), questionSchema.Type))
                        return false;
                    break;
                case AnswerSchema answerSchema:
                    if (string.IsNullOrEmpty(answerSchema.Answer) || !Enum.IsDefined(typeof(QuestionType), answerSchema.Type))
                        return false;
                    break;
                default:
                    throw new Exception(nameof(Validate));
            }

            return true;
        }

        public QuestionDto GetQuestion(QuestionType questionType)
        {
            ParsedChampion parsedChampion = _dDragonCdnService.GetRandomParsedChampion();
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
                    throw new InvalidEnumArgumentException(nameof(GetQuestion));
            }

            return questionDto;
        }

        public bool VerifyAnswer(AnswerSchema schema)
        {
            ParsedChampion parsedChampion = null;
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
                    throw new InvalidEnumArgumentException(nameof(VerifyAnswer));
            }

            if (parsedChampion == null)
                throw new KeyNotFoundException(nameof(VerifyAnswer) + " " + schema.Id);

            return schema.Answer == parsedChampion.Name;
        }
    }
}
