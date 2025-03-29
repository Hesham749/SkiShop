using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(250);
            builder.Property(u => u.LastName).HasMaxLength(250);
        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            const string AdminId = "56874229-bdfb-44cd-94b1-a5fa892fdc79";
            const string CustomerId = "6b9eec7b-38f2-4c00-a49d-66a8130982a1";

            builder.HasData(
                [
                new(){ Id = AdminId , Name = "Admin" , NormalizedName = "ADMIN" },
                new(){ Id = CustomerId , Name = "Customer" , NormalizedName = "CUSTOMER" }
                ]
                );
        }
    }
}
