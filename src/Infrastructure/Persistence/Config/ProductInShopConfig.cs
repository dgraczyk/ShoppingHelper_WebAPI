using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class ProductInShopConfig : IEntityTypeConfiguration<ProductInShop>
    {
        public void Configure(EntityTypeBuilder<ProductInShop> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.ProductId, x.ShopId })
                .IsUnique();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductInShops)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.Shop)
                .WithMany(x => x.ProductsInShop)
                .HasForeignKey(x => x.ShopId);
        }
    }
}
