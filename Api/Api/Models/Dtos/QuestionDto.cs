using Api.Models.Enums;

namespace Api.Models.Dtos
{
    public class QuestionDto
    {
        public string Id { get; set; }
        public QuestionType Type { get; set; }
        public string Value { get; set; }
    }
}
