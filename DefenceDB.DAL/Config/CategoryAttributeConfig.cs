using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models;

namespace DefenceDB.DAL.Config;

public class CategoryAttributeConfig : IEntityTypeConfiguration<CategoryAttribute>
{
    public void Configure(EntityTypeBuilder<CategoryAttribute> builder)
    {
        builder.ToTable("CategoryAttributes");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
        builder.Property(a => a.DisplayName).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Options)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
            .HasColumnType("nvarchar(max)");

        // Unique: aynı kategoride aynı isimli attribute olamaz
        builder.HasIndex(a => new { a.CategoryId, a.Name }).IsUnique();

        // Relationship to Category
        builder.HasOne(a => a.Category)
            .WithMany(c => c.Attributes)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
