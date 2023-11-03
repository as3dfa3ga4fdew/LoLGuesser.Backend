using Api.Models.Enums;
using Api.Models.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Interfaces
{
    public interface IGameService
    {
        public IActionResult GetChampionNames();
        public IActionResult GetQuestion(QuestionType questionType);
        public IActionResult VerifyAnswer(AnswerSchema schema);
    }
}
