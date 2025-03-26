using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AddressDto
    {
        [Required, MaxLength(250)]
        public string Line1 { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Line2 { get; set; }

        [Required, MaxLength(250)]
        public string City { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string State { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string PostalCode { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string Country { get; set; } = string.Empty;
    }
}
