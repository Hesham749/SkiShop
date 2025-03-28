using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, s => s.WithOwner());
            builder.OwnsOne(o => o.PaymentSummary, p => p.WithOwner());
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.Status).HasConversion(
                    s => s.ToString(),
                    s => Enum.Parse<OrderStatus>(s)
                );
            
            builder.Property(o => o.OrderDate).HasConversion(
                d => d.ToUniversalTime(),
                d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
                );

            builder.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
            builder.Property(o => o.BuyerEmail).HasMaxLength(250);
            builder.Property(o => o.PaymentIntentId).HasMaxLength(250);
        }
    }
}
