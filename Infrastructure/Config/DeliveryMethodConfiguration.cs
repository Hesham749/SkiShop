
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(d => d.Price).HasColumnType("decimal(18,2)");
            builder.Property(d => d.ShortName).HasMaxLength(200);
            builder.Property(d => d.DeliveryTime).HasMaxLength(200);
            builder.Property(d => d.Description).HasMaxLength(200);
        }
    }
}
