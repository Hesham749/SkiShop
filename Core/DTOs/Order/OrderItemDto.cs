namespace Core.DTOs.Order
{
    public record OrderItemDto
        (
            int ProductId,
            string ProductName,
            string PictureUrl,
            decimal Price,
            int Quantity
        );

}