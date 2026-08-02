using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models;

namespace DefenceDB.DAL.Config;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.IconClass).HasMaxLength(50);

        builder.HasIndex(c => c.Slug).IsUnique();

        // Self-referencing relationship
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- SEED DATA ---
        // Artık ModelTypeName yok, kategoriler tamamen dinamik.
        // Mevcut seed data ID'leri korunarak güncellenmiştir.
        var seedDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            // Ana Kategoriler
            new Category { Id = 1, Name = "Füzeler", Slug = "fuzeler", IconClass = "bi bi-rocket-takeoff", SortOrder = 1, IsShowcase = true, CreatedAt = seedDate },
            new Category { Id = 2, Name = "Savaş Uçakları", Slug = "savas-ucaklari", IconClass = "bi bi-airplane-engines", SortOrder = 2, IsShowcase = true, CreatedAt = seedDate },
            new Category { Id = 3, Name = "Savaş Gemileri", Slug = "savas-gemileri", IconClass = "bi bi-tsunami", SortOrder = 3, IsShowcase = true, CreatedAt = seedDate },
            new Category { Id = 4, Name = "Radarlar", Slug = "radarlar", IconClass = "bi bi-radar", SortOrder = 4, IsShowcase = true, CreatedAt = seedDate },

            // Füzeler Alt Kategorileri
            new Category { Id = 5, Name = "Hava-Hava Füzeleri", Slug = "hava-hava-fuzeleri", ParentCategoryId = 1, CreatedAt = seedDate },
            new Category { Id = 6, Name = "Balistik Füzeler", Slug = "balistik-fuzeler", ParentCategoryId = 1, CreatedAt = seedDate },
            new Category { Id = 7, Name = "Anti-Gemi Füzeleri", Slug = "anti-gemi-fuzeleri", ParentCategoryId = 1, CreatedAt = seedDate },
            new Category { Id = 8, Name = "Seyir Füzeleri", Slug = "seyir-fuzeleri", ParentCategoryId = 1, CreatedAt = seedDate },
            new Category { Id = 9, Name = "Anti-Radyasyon Füzeleri", Slug = "anti-radyasyon", ParentCategoryId = 1, CreatedAt = seedDate },
            new Category { Id = 10, Name = "Hipersonik Süzülme Araçları", Slug = "hgv", ParentCategoryId = 1, CreatedAt = seedDate },

            // Uçak Alt Kategorileri
            new Category { Id = 11, Name = "Avcı (Fighter)", Slug = "avci-ucaklari", ParentCategoryId = 2, CreatedAt = seedDate },
            new Category { Id = 12, Name = "Bombardıman", Slug = "bombardiman", ParentCategoryId = 2, CreatedAt = seedDate },
            new Category { Id = 13, Name = "Eğitim", Slug = "egitim", ParentCategoryId = 2, CreatedAt = seedDate },
            new Category { Id = 40, Name = "Hava SOJ", Slug = "hava-soj-ucaklari", ParentCategoryId = 2, CreatedAt = seedDate },
            new Category { Id = 41, Name = "Askeri Kargo", Slug = "askeri-kargo-ucaklari", ParentCategoryId = 2, CreatedAt = seedDate },
            new Category { Id = 42, Name = "Deniz Karakol", Slug = "deniz-karakol-ucaklari", ParentCategoryId = 2, CreatedAt = seedDate },
            new Category { Id = 43, Name = "AWACS", Slug = "awacs-ucaklari", ParentCategoryId = 2, CreatedAt = seedDate },

            // Gemi Alt Kategorileri
            new Category { Id = 14, Name = "Hücumbot", Slug = "hucumbot", ParentCategoryId = 3, CreatedAt = seedDate },
            new Category { Id = 15, Name = "Korvet", Slug = "korvet", ParentCategoryId = 3, CreatedAt = seedDate },
            new Category { Id = 16, Name = "Fırkateyn", Slug = "firkateyn", ParentCategoryId = 3, CreatedAt = seedDate },
            new Category { Id = 17, Name = "Muhrip (Destroyer)", Slug = "muhrip", ParentCategoryId = 3, CreatedAt = seedDate },
            new Category { Id = 18, Name = "Denizaltı", Slug = "denizalti", ParentCategoryId = 3, CreatedAt = seedDate },

            // Radar Alt Kategorileri
            new Category { Id = 19, Name = "Hava Savunma Radarları", Slug = "hava-savunma-radarlari", ParentCategoryId = 4, CreatedAt = seedDate },
            new Category { Id = 20, Name = "Hava Radarları (Airborne)", Slug = "airborne-radarlar", ParentCategoryId = 4, CreatedAt = seedDate },
            new Category { Id = 21, Name = "Deniz Radarları (Naval)", Slug = "deniz-radarlari", ParentCategoryId = 4, CreatedAt = seedDate },

            // Kara Araçları ve Alt Kategorileri
            new Category { Id = 22, Name = "Kara Araçları", Slug = "kara-araclari", IconClass = "bi bi-shield-shaded", SortOrder = 5, CreatedAt = seedDate },
            new Category { Id = 48, Name = "Tanklar", Slug = "tanklar", ParentCategoryId = 22, CreatedAt = seedDate },
            new Category { Id = 49, Name = "Obüs Sistemleri", Slug = "obusler", ParentCategoryId = 22, CreatedAt = seedDate },
            new Category { Id = 50, Name = "Havan Sistemleri", Slug = "havan-sistemleri", ParentCategoryId = 22, CreatedAt = seedDate },
            new Category { Id = 51, Name = "Zırhlı Personel Taşıyıcılar (ZPT)", Slug = "zpt", ParentCategoryId = 22, CreatedAt = seedDate },
            new Category { Id = 52, Name = "Zırhlı Muharebe Araçları (ZMA)", Slug = "zma", ParentCategoryId = 22, CreatedAt = seedDate },
            new Category { Id = 53, Name = "Çok Namlulu Roketatar Sistemleri (ÇNRA)", Slug = "cnra", ParentCategoryId = 22, CreatedAt = seedDate },

            // İnsansız Platformlar ve Alt Kategorileri
            new Category { Id = 23, Name = "İnsansız Platformlar", Slug = "insansiz-platformlar", IconClass = "bi bi-robot", SortOrder = 6, IsShowcase = true, CreatedAt = seedDate },
            new Category { Id = 24, Name = "İHA (UAV)", Slug = "iha-uav", ParentCategoryId = 23, CreatedAt = seedDate },
            new Category { Id = 25, Name = "İDA (USV)", Slug = "ida-usv", ParentCategoryId = 23, CreatedAt = seedDate },
            new Category { Id = 26, Name = "İKA (UGV)", Slug = "ika-ugv", ParentCategoryId = 23, CreatedAt = seedDate },
            new Category { Id = 27, Name = "Kamikaze İHA", Slug = "kamikaze-iha", ParentCategoryId = 23, CreatedAt = seedDate },
            new Category { Id = 28, Name = "Kamikaze İDA", Slug = "kamikaze-ida", ParentCategoryId = 23, CreatedAt = seedDate },

            // Motor ve Güç Sistemleri ve Alt Kategorileri
            new Category { Id = 30, Name = "Motor ve Güç Sistemleri", Slug = "motor-ve-guc-sistemleri", IconClass = "bi bi-gear-wide-connected", SortOrder = 7, CreatedAt = seedDate },
            new Category { Id = 31, Name = "Turbofan Motorlar", Slug = "turbofan-motorlar", ParentCategoryId = 30, CreatedAt = seedDate },
            new Category { Id = 32, Name = "Pistonlu/İçten Yanmalı Motorlar", Slug = "pistonlu-motorlar", ParentCategoryId = 30, CreatedAt = seedDate },
            new Category { Id = 33, Name = "Roket Motorları", Slug = "roket-motorlari", ParentCategoryId = 30, CreatedAt = seedDate },
            new Category { Id = 34, Name = "Elektrik ve Nükleer Güç", Slug = "elektrik-ve-nukleer-guc", ParentCategoryId = 30, CreatedAt = seedDate },
            new Category { Id = 35, Name = "Turbojet Motorlar", Slug = "turbojet-motorlar", ParentCategoryId = 30, CreatedAt = seedDate },
            new Category { Id = 36, Name = "Turboprop Motorlar", Slug = "turboprop-motorlar", ParentCategoryId = 30, CreatedAt = seedDate },
            new Category { Id = 37, Name = "Deniz Gaz Türbinleri", Slug = "deniz-gaz-turbinleri", ParentCategoryId = 30, CreatedAt = seedDate },
            new Category { Id = 38, Name = "Turboshaft Motorlar", Slug = "turboshaft-motorlar", ParentCategoryId = 30, CreatedAt = seedDate },

            // Hava Savunma Sistemleri ve Alt Kategorileri
            new Category { Id = 39, Name = "Hava Savunma Sistemleri", Slug = "hava-savunma-sistemleri", IconClass = "bi bi-shield-fill-check", SortOrder = 8, IsShowcase = true, CreatedAt = seedDate },
            new Category { Id = 44, Name = "Taşınabilir Hava Savunma Sistemleri (MANPADS)", Slug = "manpads", ParentCategoryId = 39, CreatedAt = seedDate },
            new Category { Id = 45, Name = "Yakın Savunma Silah Sistemleri (CIWS)", Slug = "ciws", ParentCategoryId = 39, CreatedAt = seedDate },
            new Category { Id = 46, Name = "Kundağı Motorlu Uçaksavar Topları (SPAAG)", Slug = "spaag", ParentCategoryId = 39, CreatedAt = seedDate },
            new Category { Id = 47, Name = "Hava ve Füze Savunma Sistemleri", Slug = "hava-ve-fuze-savunma-sistemleri", ParentCategoryId = 39, CreatedAt = seedDate },

            // Piyade Silahları ve Alt Kategorileri
            new Category { Id = 54, Name = "Piyade Silahları", Slug = "piyade-silahlari", IconClass = "bi bi-crosshair", SortOrder = 9, CreatedAt = seedDate },
            new Category { Id = 55, Name = "Tabancalar", Slug = "tabancalar", ParentCategoryId = 54, CreatedAt = seedDate },
            new Category { Id = 56, Name = "Piyade Tüfekleri", Slug = "piyade-tufekleri", ParentCategoryId = 54, CreatedAt = seedDate },
            new Category { Id = 57, Name = "Makineli Tüfekler", Slug = "makineli-tufekler", ParentCategoryId = 54, CreatedAt = seedDate },
            new Category { Id = 58, Name = "Keskin Nişancı Tüfekleri", Slug = "keskin-nisanci-tufekleri", ParentCategoryId = 54, CreatedAt = seedDate }
        );
    }
}
