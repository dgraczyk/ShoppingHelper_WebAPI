using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Vendor)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Size)                
                .HasPrecision(5, 2);

            builder.Property(x => x.SizeUnit)                
                .HasConversion<string>();

            builder.Property(x => x.CategoryId)
                .IsRequired();

            builder.Property(x => x.Hash)
                .IsRequired();
        }
    }
}
