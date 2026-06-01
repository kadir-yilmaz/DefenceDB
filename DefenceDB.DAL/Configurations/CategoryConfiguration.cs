using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models;

namespace DefenceDB.DAL.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.IconClass).HasMaxLength(50);

        // Self-referencing relationship
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- SEED DATA ---
        builder.HasData(
            // Ana Kategoriler (1-4)
            new Category { Id = 1, Name = "Füzeler", Slug = "fuzeler", IconClass = "bi bi-rocket-takeoff", SortOrder = 1, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 2, Name = "Savaş Uçakları", Slug = "savas-ucaklari", IconClass = "bi bi-airplane-engines", SortOrder = 2, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 3, Name = "Savaş Gemileri", Slug = "savas-gemileri", IconClass = "bi bi-tsunami", SortOrder = 3, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 4, Name = "Radarlar", Slug = "radarlar", IconClass = "bi bi-radar", SortOrder = 4, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 22, Name = "Tanklar", Slug = "tanklar", IconClass = "bi bi-shield-shaded", SortOrder = 5, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Füzeler Alt Kategorileri (5-10)
            new Category { Id = 5, Name = "Hava-Hava Füzeleri", Slug = "hava-hava-fuzeleri", ParentCategoryId = 1, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 6, Name = "Balistik Füzeler", Slug = "balistik-fuzeler", ParentCategoryId = 1, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 7, Name = "Anti-Gemi Füzeleri", Slug = "anti-gemi-fuzeleri", ParentCategoryId = 1, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 8, Name = "Seyir Füzeleri", Slug = "seyir-fuzeleri", ParentCategoryId = 1, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 9, Name = "Anti-Radyasyon Füzeleri", Slug = "anti-radyasyon", ParentCategoryId = 1, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 10, Name = "Hipersonik Süzülme Araçları", Slug = "hgv", ParentCategoryId = 1, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Uçak Alt Kategorileri (11-13)
            new Category { Id = 11, Name = "Avcı (Fighter)", Slug = "avci-ucaklari", ParentCategoryId = 2, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 12, Name = "Bombardıman", Slug = "bombardiman", ParentCategoryId = 2, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 13, Name = "Eğitim", Slug = "egitim", ParentCategoryId = 2, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Gemi Alt Kategorileri (14-16)
            new Category { Id = 14, Name = "Hücumbot", Slug = "hucumbot", ParentCategoryId = 3, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 15, Name = "Korvet", Slug = "korvet", ParentCategoryId = 3, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 16, Name = "Fırkateyn", Slug = "firkateyn", ParentCategoryId = 3, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // Yeni Eklenen Alt Kategoriler: Gemiler (17-18), Radarlar (19-21)
            new Category { Id = 17, Name = "Muhrip (Destroyer)", Slug = "muhrip", ParentCategoryId = 3, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 18, Name = "Denizaltı", Slug = "denizalti", ParentCategoryId = 3, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            new Category { Id = 19, Name = "Hava Savunma Radarları", Slug = "hava-savunma-radarlari", ParentCategoryId = 4, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 20, Name = "Hava Radarları (Airborne)", Slug = "airborne-radarlar", ParentCategoryId = 4, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 21, Name = "Deniz Radarları (Naval)", Slug = "deniz-radarlari", ParentCategoryId = 4, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
