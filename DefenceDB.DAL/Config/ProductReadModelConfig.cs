using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models;

namespace DefenceDB.DAL.Config;

public class ProductReadModelConfig : IEntityTypeConfiguration<ProductReadModel>
{
    public void Configure(EntityTypeBuilder<ProductReadModel> builder)
    {
        builder.ToTable("ProductReadModels");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Slug).IsRequired().HasMaxLength(200);
        builder.Property(p => p.NatoReportingName).HasMaxLength(100);
        builder.Property(p => p.Country).HasMaxLength(100);
        builder.Property(p => p.Manufacturer).HasMaxLength(200);
        builder.Property(p => p.Status).HasMaxLength(50);
        builder.Property(p => p.VideoUrl).HasMaxLength(500);
        builder.Property(p => p.CategoryName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.CategorySlug).IsRequired().HasMaxLength(100);
        builder.Property(p => p.ProductType).IsRequired().HasMaxLength(100);
        builder.Property(p => p.SpecificPropertiesJson).IsRequired();

        // Indexes for query performance
        builder.HasIndex(p => p.Slug);
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.Country);
        builder.HasIndex(p => p.ProductType);
        builder.HasIndex(p => new { p.IsActive, p.IsShowcase });
        builder.HasIndex(p => p.CreatedAt);
    }
}
