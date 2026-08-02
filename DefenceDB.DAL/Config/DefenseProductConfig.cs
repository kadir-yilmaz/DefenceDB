using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models;

namespace DefenceDB.DAL.Config;

public class DefenseProductConfig : IEntityTypeConfiguration<DefenseProduct>
{
    public void Configure(EntityTypeBuilder<DefenseProduct> builder)
    {
        builder.ToTable("DefenseProducts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Slug).IsRequired().HasMaxLength(250);
        builder.Property(p => p.NatoReportingName).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(5000);
        builder.Property(p => p.Country).HasMaxLength(100);
        builder.Property(p => p.Manufacturer).HasMaxLength(200);
        builder.Property(p => p.ThumbnailUrl).HasMaxLength(500);
        builder.Property(p => p.Status).HasMaxLength(50);
        builder.Property(p => p.VideoUrl).HasMaxLength(500);

        // Specs JSON column — dinamik kategoriye özel alanlar
        var jsonOptions = new System.Text.Json.JsonSerializerOptions();
        jsonOptions.Converters.Add(new StringDictionaryConverter());

        builder.Property(p => p.Specs)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, string>>(v, jsonOptions))
            .HasColumnType("nvarchar(max)");

        // Indexes
        builder.HasIndex(p => p.Slug).IsUnique();
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.Country);
        builder.HasIndex(p => new { p.IsActive, p.IsShowcase });
        builder.HasIndex(p => p.CreatedAt);

        // Relationship to Category
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
