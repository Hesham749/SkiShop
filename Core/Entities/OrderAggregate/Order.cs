using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Entities.OrderAggregate
{
    public class Order : BaseEntity , IDtoConvertible
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public required string BuyerEmail { get; set; }

        public ShippingAddress? ShippingAddress { get; set; }

        public DeliveryMethod? DeliveryMethod { get; set; }

        public PaymentSummary? PaymentSummary { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total => Subtotal + (DeliveryMethod?.Price ?? 0);

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public required string PaymentIntentId { get; set; }


        public required IReadOnlyList<OrderItem> OrderItems { get; set; }
    }
}
