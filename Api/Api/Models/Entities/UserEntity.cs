using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public int Score { get; set; }

        public ICollection<AddressEntity> Addresses { get; set; }
    }
}
