using Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Schemas
{
    public class QuestionSchema
    {
        [Required]
        public QuestionType Type { get; set; }
    }
}
