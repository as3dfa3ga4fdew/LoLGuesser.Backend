using Api.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Schemas
{
    public class AddressSchema
    {
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
    }
}
