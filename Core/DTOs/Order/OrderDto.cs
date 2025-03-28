using Core.Entities.OrderAggregate;

namespace Core.DTOs.Order
{
    public record OrderDto
        (
            int Id,

            DateTime OrderDate,

            string BuyerEmail,

            ShippingAddress ShippingAddress,

            string DeliveryMethod,

            decimal ShippingPrice,

            PaymentSummary PaymentSummary,

            IReadOnlyCollection<OrderItemDto> OrderItems,

            decimal Subtotal,

            decimal Total,

            string Status,

            string PaymentIntentId

        );


}
