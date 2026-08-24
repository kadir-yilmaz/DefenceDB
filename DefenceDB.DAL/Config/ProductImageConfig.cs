using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models;

namespace DefenceDB.DAL.Config;

public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.ImagePath).IsRequired();

        // Index on ProductId and composite index on (ProductId, IsMainImage) for fast joins and main image lookups
        builder.HasIndex(i => i.ProductId);
        builder.HasIndex(i => new { i.ProductId, i.IsMainImage });

        builder.HasOne(i => i.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
