using Api.Models.Dtos;
using Api.Models.Enums;
using Api.Models.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Interfaces
{
    public interface IGameService
    {
        public IEnumerable<string> GetChampionNames();
        public bool Validate<TSchema>(TSchema schema);
        public QuestionDto GetQuestion(QuestionType questionType);
        public bool VerifyAnswer(AnswerSchema schema);
    }
}
