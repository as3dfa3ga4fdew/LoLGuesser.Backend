using Api.Models.Classes;
using Api.Models.Dtos;
using Api.Models.Enums;
using Api.Models.Schemas;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace Api.Services.Interfaces
{
    public interface IGameService
    {
        public IImmutableList<string> GetChampionNames();
        public bool Validate<TSchema>(TSchema schema);
        public QuestionDto GetQuestion(QuestionType questionType);
        public bool VerifyAnswer(AnswerSchema schema);
        public ParsedChampion GetParsedChampionById(QuestionType type, string id);
    }
}
