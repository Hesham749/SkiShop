using System.ComponentModel.DataAnnotations;
using Core.Entities.OrderAggregate;

namespace Core.DTOs.Order
{
    public record CreateOrderDto(
        string CartId,
        [Required] int? DeliveryMethodId,
        ShippingAddress ShippingAddress,
        PaymentSummary PaymentSummary
        );

}
