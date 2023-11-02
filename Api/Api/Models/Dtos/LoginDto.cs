using System.ComponentModel.DataAnnotations;

namespace Api.Models.Dtos
{
    public class LoginDto
    {
        public string Jwt { get; set; } = null!;
        public string Username { get; set; } = null!;
        public int Score { get; set; }
    }
}
