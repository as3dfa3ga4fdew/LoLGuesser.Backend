using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    public class AddressEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Street { get; set; } = null!;
        [Required]
        public string PostalCode { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;

        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
