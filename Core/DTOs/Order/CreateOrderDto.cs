using System.ComponentModel.DataAnnotations;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Core.DTOs.Order
{
    public record CreateOrderDto(
        string CartId,
        [Required] int? DeliveryMethodId,
        ShippingAddress ShippingAddress,
        PaymentSummary PaymentSummary
        ) : IDto;

}
