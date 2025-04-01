using System.ComponentModel.DataAnnotations;
using Core.Interfaces;

namespace Core.DTOs
{
    public record RegisterDto
    (
        [Required]

         string FirstName,

        [Required]
         string LastName,

        [Required]
        [EmailAddress]
         string Email,

        [Required]
         string Password

    ) : IDto;
}
