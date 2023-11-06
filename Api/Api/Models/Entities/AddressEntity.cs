using Api.Models.Dtos;
using Api.Models.Schemas;
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

        public static implicit operator AddressDto(AddressEntity entity)
        {
            return new AddressDto()
            {
                Id = entity.Id,
                Title = entity.Title,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Street = entity.Street,
                PostalCode = entity.PostalCode,
                City = entity.City
            };
        }
    }
}
