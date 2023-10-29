using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Score { get; set; }
    }
}
