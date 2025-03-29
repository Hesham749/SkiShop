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


        //public static OrderDto ToDto(this Order order)
        //{
        //    return new OrderDto
        //    {
        //        Id = order.Id,
        //        BuyerEmail = order.BuyerEmail,
        //        OrderDate = order.OrderDate,
        //        ShippingAddress = order.ShippingAddress!,
        //        PaymentSummary = order.PaymentSummary!,
        //        DeliveryMethod = order.DeliveryMethod!.Description,
        //        ShippingPrice = order.DeliveryMethod.Price,
        //        OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
        //        Subtotal = order.Subtotal,
        //        Total = order.Total,
        //        Status = order.Status.ToString(),
        //        PaymentIntentId = order.PaymentIntentId
        //    };
        //}

        //public static OrderItemDto ToDto(this OrderItem orderItem)
        //{
        //    return new OrderItemDto
        //    {
        //        ProductId = orderItem.ItemOrdered.ProductId,
        //        ProductName = orderItem.ItemOrdered.ProductName,
        //        PictureUrl = orderItem.ItemOrdered.PictureUrl,
        //        Price = orderItem.Price,
        //        Quantity = orderItem.Quantity
        //    };
        //}

    }
}
