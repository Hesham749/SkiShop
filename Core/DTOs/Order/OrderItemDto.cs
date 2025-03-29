using Core.Interfaces;

namespace Core.DTOs.Order
{
    public record OrderItemDto
        (
            int ProductId,
            string ProductName,
            string PictureUrl,
            decimal Price,
            int Quantity
        ) : IDto;


    //public class OrderItemDto : IDto
    //{
    //    public int ProductId { get; set; }
    //    public required string ProductName { get; set; }
    //    public required string PictureUrl { get; set; }
    //    public decimal Price { get; set; }
    //    public int Quantity { get; set; }
    //}
}