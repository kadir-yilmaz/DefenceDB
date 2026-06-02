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
            // Füzeler Alt Kategorileri (5-10)
            new Category { Id = 5, Name = "Hava-Hava Füzeleri", Slug = "hava-hava-fuzeleri", ParentCategoryId = 1, ModelTypeName = "DefenceDB.EL.Models.Products.AirToAirMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 6, Name = "Balistik Füzeler", Slug = "balistik-fuzeler", ParentCategoryId = 1, ModelTypeName = "DefenceDB.EL.Models.Products.BallisticMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 7, Name = "Anti-Gemi Füzeleri", Slug = "anti-gemi-fuzeleri", ParentCategoryId = 1, ModelTypeName = "DefenceDB.EL.Models.Products.AntiShipMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 8, Name = "Seyir Füzeleri", Slug = "seyir-fuzeleri", ParentCategoryId = 1, ModelTypeName = "DefenceDB.EL.Models.Products.CruiseMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 9, Name = "Anti-Radyasyon Füzeleri", Slug = "anti-radyasyon", ParentCategoryId = 1, ModelTypeName = "DefenceDB.EL.Models.Products.AntiRadiationMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 10, Name = "Hipersonik Süzülme Araçları", Slug = "hgv", ParentCategoryId = 1, ModelTypeName = "DefenceDB.EL.Models.Products.HypersonicGlideVehicle", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Uçak Alt Kategorileri (11-13)
            new Category { Id = 11, Name = "Avcı (Fighter)", Slug = "avci-ucaklari", ParentCategoryId = 2, ModelTypeName = "DefenceDB.EL.Models.Products.FighterAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 12, Name = "Bombardıman", Slug = "bombardiman", ParentCategoryId = 2, ModelTypeName = "DefenceDB.EL.Models.Products.BomberAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 13, Name = "Eğitim", Slug = "egitim", ParentCategoryId = 2, ModelTypeName = "DefenceDB.EL.Models.Products.TrainerAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Gemi Alt Kategorileri (14-16)
            new Category { Id = 14, Name = "Hücumbot", Slug = "hucumbot", ParentCategoryId = 3, ModelTypeName = "DefenceDB.EL.Models.Products.FastAttackCraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 15, Name = "Korvet", Slug = "korvet", ParentCategoryId = 3, ModelTypeName = "DefenceDB.EL.Models.Products.Corvette", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 16, Name = "Fırkateyn", Slug = "firkateyn", ParentCategoryId = 3, ModelTypeName = "DefenceDB.EL.Models.Products.Frigate", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // Yeni Eklenen Alt Kategoriler: Gemiler (17-18), Radarlar (19-21)
            new Category { Id = 17, Name = "Muhrip (Destroyer)", Slug = "muhrip", ParentCategoryId = 3, ModelTypeName = "DefenceDB.EL.Models.Products.Destroyer", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 18, Name = "Denizaltı", Slug = "denizalti", ParentCategoryId = 3, ModelTypeName = "DefenceDB.EL.Models.Products.Submarine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            new Category { Id = 19, Name = "Hava Savunma Radarları", Slug = "hava-savunma-radarlari", ParentCategoryId = 4, ModelTypeName = "DefenceDB.EL.Models.Products.AirDefenseRadar", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 20, Name = "Hava Radarları (Airborne)", Slug = "airborne-radarlar", ParentCategoryId = 4, ModelTypeName = "DefenceDB.EL.Models.Products.AirborneRadar", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 21, Name = "Deniz Radarları (Naval)", Slug = "deniz-radarlari", ParentCategoryId = 4, ModelTypeName = "DefenceDB.EL.Models.Products.NavalRadar", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            new Category { Id = 22, Name = "Tanklar", Slug = "tanklar", IconClass = "bi bi-shield-shaded", SortOrder = 5, ModelTypeName = "DefenceDB.EL.Models.Products.Tank", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // İnsansız Platformlar (23) ve Alt Kategorileri (24-29)
            new Category { Id = 23, Name = "İnsansız Platformlar", Slug = "insansiz-platformlar", IconClass = "bi bi-robot", SortOrder = 6, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 24, Name = "İHA (UAV)", Slug = "iha-uav", ParentCategoryId = 23, ModelTypeName = "DefenceDB.EL.Models.Products.UAV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 25, Name = "İDA (USV)", Slug = "ida-usv", ParentCategoryId = 23, ModelTypeName = "DefenceDB.EL.Models.Products.USV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 26, Name = "İKA (UGV)", Slug = "ika-ugv", ParentCategoryId = 23, ModelTypeName = "DefenceDB.EL.Models.Products.UGV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 27, Name = "Kamikaze İHA", Slug = "kamikaze-iha", ParentCategoryId = 23, ModelTypeName = "DefenceDB.EL.Models.Products.KamikazeUAV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 28, Name = "Kamikaze İDA", Slug = "kamikaze-ida", ParentCategoryId = 23, ModelTypeName = "DefenceDB.EL.Models.Products.KamikazeUSV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // Motor ve Güç Sistemleri (30) ve Alt Kategorileri
            new Category { Id = 30, Name = "Motor ve Güç Sistemleri", Slug = "motor-ve-guc-sistemleri", IconClass = "bi bi-gear-wide-connected", SortOrder = 7, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 31, Name = "Turbofan Motorlar", Slug = "turbofan-motorlar", ParentCategoryId = 30, ModelTypeName = "DefenceDB.EL.Models.Products.TurbofanEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 35, Name = "Turbojet Motorlar", Slug = "turbojet-motorlar", ParentCategoryId = 30, ModelTypeName = "DefenceDB.EL.Models.Products.TurbojetEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 36, Name = "Turboprop Motorlar", Slug = "turboprop-motorlar", ParentCategoryId = 30, ModelTypeName = "DefenceDB.EL.Models.Products.TurbopropEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 38, Name = "Turboshaft Motorlar", Slug = "turboshaft-motorlar", ParentCategoryId = 30, ModelTypeName = "DefenceDB.EL.Models.Products.TurboshaftEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 37, Name = "Deniz Gaz Türbinleri", Slug = "deniz-gaz-turbinleri", ParentCategoryId = 30, ModelTypeName = "DefenceDB.EL.Models.Products.MarineGasTurbine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 32, Name = "Pistonlu/İçten Yanmalı Motorlar", Slug = "pistonlu-motorlar", ParentCategoryId = 30, ModelTypeName = "DefenceDB.EL.Models.Products.PistonEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 33, Name = "Roket Motorları", Slug = "roket-motorlari", ParentCategoryId = 30, ModelTypeName = "DefenceDB.EL.Models.Products.RocketMotor", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 34, Name = "Elektrik ve Nükleer Güç", Slug = "elektrik-ve-nukleer-guc", ParentCategoryId = 30, ModelTypeName = "DefenceDB.EL.Models.Products.ElectricNuclearPower", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
