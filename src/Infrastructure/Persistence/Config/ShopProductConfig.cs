using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class ShopProductConfig : IEntityTypeConfiguration<ShopProduct>
    {
        public void Configure(EntityTypeBuilder<ShopProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.ProductId, x.ShopId })
                .IsUnique();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ShopProducts)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.Shop)
                .WithMany(x => x.ShopProducts)
                .HasForeignKey(x => x.ShopId);
        }
    }
}
