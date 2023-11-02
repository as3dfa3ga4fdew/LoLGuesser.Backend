using System.ComponentModel.DataAnnotations;

namespace Api.Models.Schemas
{
    public class LoginSchema
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
