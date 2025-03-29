using Core.Entities.OrderAggregate;
using Core.Interfaces;

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

        ) : IDto;


    //public class OrderDto : IDto
    //{
    //    public int Id { get; set; }
    //    public DateTime OrderDate { get; set; }
    //    public required string BuyerEmail { get; set; }
    //    public required ShippingAddress ShippingAddress { get; set; }
    //    public required string DeliveryMethod { get; set; }
    //    public decimal ShippingPrice { get; set; }
    //    public required PaymentSummary PaymentSummary { get; set; }
    //    public required IReadOnlyCollection<OrderItemDto> OrderItems { get; set; }
    //    public decimal Subtotal { get; set; }
    //    public required string Status { get; set; }
    //    public decimal Total { get; set; }
    //    public required string PaymentIntentId { get; set; }
    //}

}
