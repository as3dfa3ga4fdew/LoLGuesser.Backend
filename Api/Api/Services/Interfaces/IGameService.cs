using Api.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Interfaces
{
    public interface IGameService
    {
        public IActionResult GetQuestion(QuestionType questionType);
    }
}
