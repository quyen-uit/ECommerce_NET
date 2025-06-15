using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class AddressDto
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string HouseNumber { get; set; }
        [Required]
        public required string Street { get; set; }
        [Required]
        public required string Ward { get; set; }
        [Required]
        public required string District { get; set; }
        [Required]
        public required string City { get; set; }
        [Required]
        public int ZipCode { get; set; }
    }
}
