using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class PriceConfig : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.Property(x => x.PriceValue)
                .IsRequired()
                .HasPrecision(8, 2);

            builder.Property(x => x.PricePerSizeUnit)                
                .HasPrecision(8, 2);

            builder.Property(x => x.SizeUnit)
               .HasConversion<string>();

            builder.Property(x => x.PromotionConstraints)
               .HasMaxLength(500);

            builder.Property(x => x.ShopProductId)
               .IsRequired();

            builder.Property(x => x.Created)
               .HasDefaultValueSql("getdate()");
        }
    }
}
