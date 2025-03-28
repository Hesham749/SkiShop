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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(o => o.ItemOrdered, io =>
            {
                io.WithOwner();
                io.Property(io => io.ProductName).HasMaxLength(250);
                io.Property(io => io.PictureUrl).HasMaxLength(250);
            });



            builder.Property(o => o.Price).HasColumnType("decimal(18,2)");

        }
    }
}
