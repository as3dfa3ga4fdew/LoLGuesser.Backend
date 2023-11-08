using System.ComponentModel.DataAnnotations;

namespace Api.Models.Dtos
{
    public class LoginDto : Dto
    {
        public string Username { get; set; } = null!;
        public int Score { get; set; }
        public string Jwt { get; set; } = null!;
    }
}
