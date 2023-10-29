using Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Schemas
{
    public class AnswerSchema
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public QuestionType Type { get; set; }
        [Required]
        public string Answer { get; set; } = null!;
    }
}
