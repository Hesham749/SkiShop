using System.ComponentModel.DataAnnotations;

namespace API.DTOs
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

    );
}
