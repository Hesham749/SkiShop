using System.ComponentModel.DataAnnotations;
using Core.Interfaces;

namespace Core.DTOs
{
    public record AddressDto
    (
        [Required, MaxLength(250)]
         string Line1,

        [MaxLength(250)]
         string? Line2,

        [Required, MaxLength(250)]
         string City,

        [Required, MaxLength(250)]
         string State,

        [Required, MaxLength(250)]
         string PostalCode,

        [Required, MaxLength(250)]
         string Country
    ) : IDto;
}
