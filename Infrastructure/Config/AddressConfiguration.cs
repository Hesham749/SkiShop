using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");
            builder.Property(a => a.Line1).HasMaxLength(250);
            builder.Property(a => a.Line2).HasMaxLength(250);
            builder.Property(a => a.PostalCode).HasMaxLength(250);
            builder.Property(a => a.State).HasMaxLength(250);
            builder.Property(a => a.City).HasMaxLength(250);
            builder.Property(a => a.Country).HasMaxLength(250);
        }
    }
}
