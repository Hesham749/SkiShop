using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateProductDto
    {
        [Required, StringLength(250, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(250, MinimumLength = 3)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required, StringLength(250, MinimumLength = 3)]
        public string PictureUrl { get; set; } = string.Empty;

        [Required, StringLength(250, MinimumLength = 3)]
        public string Type { get; set; } = string.Empty;

        [Required, StringLength(250, MinimumLength = 3)]
        public string Brand { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be at least 1")]
        public int QuantityInStock { get; set; }
    }
}
