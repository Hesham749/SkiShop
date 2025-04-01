using System.ComponentModel.DataAnnotations;
using Core.Interfaces;

namespace Core.DTOs
{
    public record CreateProductDto
    (
        [Required, StringLength(250, MinimumLength = 3)]
         string Name,

        [Required, StringLength(250, MinimumLength = 3)]
         string Description,

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
         decimal Price,

        [Required, StringLength(250, MinimumLength = 3)]
         string PictureUrl,

        [Required, StringLength(250, MinimumLength = 3)]
         string Type,

        [Required, StringLength(250, MinimumLength = 3)]
         string Brand,
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be at least 1")]
         int QuantityInStock
    ) : IDto;
}
