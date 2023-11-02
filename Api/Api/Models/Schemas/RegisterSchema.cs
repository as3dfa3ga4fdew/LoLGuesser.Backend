using System.ComponentModel.DataAnnotations;

namespace Api.Models.Schemas
{
    public class RegisterSchema
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
