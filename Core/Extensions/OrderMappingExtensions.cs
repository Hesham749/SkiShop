using Core.Entities.OrderAggregate;

namespace Core.Extensions
{
    public static class OrderMappingExtensions
    {
        public static OrderDto ToDto(this Order order)
        {
            return new
                 (
                 order.Id,
                 order.OrderDate,
                 order.BuyerEmail,
                 order.ShippingAddress!,
                 order.DeliveryMethod!.ShortName,
                 order.DeliveryMethod.Price,
                 order.PaymentSummary!,
                 [.. order.OrderItems.Select(i => i.ToDto())],
                 order.Subtotal,
                 order.Total,
                 order.Status.ToString(),
                 order.PaymentIntentId

                 );
        }


        public static OrderItemDto ToDto(this OrderItem orderItem)
        {
            return new OrderItemDto
                (
                    orderItem.ItemOrdered.ProductId,
                    orderItem.ItemOrdered.ProductName,
                    orderItem.ItemOrdered.PictureUrl,
                    orderItem.Price,
                    orderItem.Quantity

                );

        }


    }
}
