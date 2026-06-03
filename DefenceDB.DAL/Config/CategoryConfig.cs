using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models;
using DefenceDB.EL.Constants;

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

        // Self-referencing relationship
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- SEED DATA ---
        builder.HasData(
            // Ana Kategoriler (1-4)
            new Category { Id = CategoryConstants.Fuzeler, Name = "Füzeler", Slug = "fuzeler", IconClass = "bi bi-rocket-takeoff", SortOrder = 1, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.SavasUcaklari, Name = "Savaş Uçakları", Slug = "savas-ucaklari", IconClass = "bi bi-airplane-engines", SortOrder = 2, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.SavasGemileri, Name = "Savaş Gemileri", Slug = "savas-gemileri", IconClass = "bi bi-tsunami", SortOrder = 3, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Radarlar, Name = "Radarlar", Slug = "radarlar", IconClass = "bi bi-radar", SortOrder = 4, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // Füzeler Alt Kategorileri (5-10)
            new Category { Id = CategoryConstants.HavaHavaFuzeleri, Name = "Hava-Hava Füzeleri", Slug = "hava-hava-fuzeleri", ParentCategoryId = CategoryConstants.Fuzeler, ModelTypeName = "DefenceDB.EL.Models.Products.AirToAirMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.BalistikFuzeler, Name = "Balistik Füzeler", Slug = "balistik-fuzeler", ParentCategoryId = CategoryConstants.Fuzeler, ModelTypeName = "DefenceDB.EL.Models.Products.BallisticMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.AntiGemiFuzeleri, Name = "Anti-Gemi Füzeleri", Slug = "anti-gemi-fuzeleri", ParentCategoryId = CategoryConstants.Fuzeler, ModelTypeName = "DefenceDB.EL.Models.Products.AntiShipMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.SeyirFuzeleri, Name = "Seyir Füzeleri", Slug = "seyir-fuzeleri", ParentCategoryId = CategoryConstants.Fuzeler, ModelTypeName = "DefenceDB.EL.Models.Products.CruiseMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.AntiRadyasyonFuzeleri, Name = "Anti-Radyasyon Füzeleri", Slug = "anti-radyasyon", ParentCategoryId = CategoryConstants.Fuzeler, ModelTypeName = "DefenceDB.EL.Models.Products.AntiRadiationMissile", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.HipersonikSuzulmeAraclari, Name = "Hipersonik Süzülme Araçları", Slug = "hgv", ParentCategoryId = CategoryConstants.Fuzeler, ModelTypeName = "DefenceDB.EL.Models.Products.HypersonicGlideVehicle", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Uçak Alt Kategorileri (11-13, 40-43)
            new Category { Id = CategoryConstants.AvciUcaklari, Name = "Avcı (Fighter)", Slug = "avci-ucaklari", ParentCategoryId = CategoryConstants.SavasUcaklari, ModelTypeName = "DefenceDB.EL.Models.Products.FighterAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.BombardimanUcaklari, Name = "Bombardıman", Slug = "bombardiman", ParentCategoryId = CategoryConstants.SavasUcaklari, ModelTypeName = "DefenceDB.EL.Models.Products.BomberAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.EgitimUcaklari, Name = "Eğitim", Slug = "egitim", ParentCategoryId = CategoryConstants.SavasUcaklari, ModelTypeName = "DefenceDB.EL.Models.Products.TrainerAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.HavaSojUcaklari, Name = "Hava SOJ", Slug = "hava-soj-ucaklari", ParentCategoryId = CategoryConstants.SavasUcaklari, ModelTypeName = "DefenceDB.EL.Models.Products.AirSojAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.AskeriKargoUcaklari, Name = "Askeri Kargo", Slug = "askeri-kargo-ucaklari", ParentCategoryId = CategoryConstants.SavasUcaklari, ModelTypeName = "DefenceDB.EL.Models.Products.CargoAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.DenizKarakolUcaklari, Name = "Deniz Karakol", Slug = "deniz-karakol-ucaklari", ParentCategoryId = CategoryConstants.SavasUcaklari, ModelTypeName = "DefenceDB.EL.Models.Products.MaritimePatrolAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.AwacsUcaklari, Name = "AWACS", Slug = "awacs-ucaklari", ParentCategoryId = CategoryConstants.SavasUcaklari, ModelTypeName = "DefenceDB.EL.Models.Products.AwacsAircraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Gemi Alt Kategorileri (14-18)
            new Category { Id = CategoryConstants.Hucumbot, Name = "Hücumbot", Slug = "hucumbot", ParentCategoryId = CategoryConstants.SavasGemileri, ModelTypeName = "DefenceDB.EL.Models.Products.FastAttackCraft", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Korvet, Name = "Korvet", Slug = "korvet", ParentCategoryId = CategoryConstants.SavasGemileri, ModelTypeName = "DefenceDB.EL.Models.Products.Corvette", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Firkateyn, Name = "Fırkateyn", Slug = "firkateyn", ParentCategoryId = CategoryConstants.SavasGemileri, ModelTypeName = "DefenceDB.EL.Models.Products.Frigate", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Muhrip, Name = "Muhrip (Destroyer)", Slug = "muhrip", ParentCategoryId = CategoryConstants.SavasGemileri, ModelTypeName = "DefenceDB.EL.Models.Products.Destroyer", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Denizalti, Name = "Denizaltı", Slug = "denizalti", ParentCategoryId = CategoryConstants.SavasGemileri, ModelTypeName = "DefenceDB.EL.Models.Products.Submarine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // Radar Alt Kategorileri (19-21)
            new Category { Id = CategoryConstants.HavaSavunmaRadarlari, Name = "Hava Savunma Radarları", Slug = "hava-savunma-radarlari", ParentCategoryId = CategoryConstants.Radarlar, ModelTypeName = "DefenceDB.EL.Models.Products.AirDefenseRadar", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.AirborneRadarlar, Name = "Hava Radarları (Airborne)", Slug = "airborne-radarlar", ParentCategoryId = CategoryConstants.Radarlar, ModelTypeName = "DefenceDB.EL.Models.Products.AirborneRadar", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.DenizRadarlari, Name = "Deniz Radarları (Naval)", Slug = "deniz-radarlari", ParentCategoryId = CategoryConstants.Radarlar, ModelTypeName = "DefenceDB.EL.Models.Products.NavalRadar", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // Kara Araçları (22) ve Alt Kategorileri (48-53)
            new Category { Id = CategoryConstants.KaraAraclari, Name = "Kara Araçları", Slug = "kara-araclari", IconClass = "bi bi-shield-shaded", SortOrder = 5, ModelTypeName = null, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Tanklar, Name = "Tanklar", Slug = "tanklar", ParentCategoryId = CategoryConstants.KaraAraclari, ModelTypeName = "DefenceDB.EL.Models.Products.LandVehicle", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Obusler, Name = "Obüs Sistemleri", Slug = "obusler", ParentCategoryId = CategoryConstants.KaraAraclari, ModelTypeName = "DefenceDB.EL.Models.Products.LandVehicle", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.HavanSistemleri, Name = "Havan Sistemleri", Slug = "havan-sistemleri", ParentCategoryId = CategoryConstants.KaraAraclari, ModelTypeName = "DefenceDB.EL.Models.Products.LandVehicle", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Zpt, Name = "Zırhlı Personel Taşıyıcılar (ZPT)", Slug = "zpt", ParentCategoryId = CategoryConstants.KaraAraclari, ModelTypeName = "DefenceDB.EL.Models.Products.LandVehicle", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Zma, Name = "Zırhlı Muharebe Araçları (ZMA)", Slug = "zma", ParentCategoryId = CategoryConstants.KaraAraclari, ModelTypeName = "DefenceDB.EL.Models.Products.LandVehicle", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Cnra, Name = "Çok Namlulu Roketatar Sistemleri (ÇNRA)", Slug = "cnra", ParentCategoryId = CategoryConstants.KaraAraclari, ModelTypeName = "DefenceDB.EL.Models.Products.LandVehicle", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // İnsansız Platformlar (23) ve Alt Kategorileri (24-28)
            new Category { Id = CategoryConstants.InsansizPlatformlar, Name = "İnsansız Platformlar", Slug = "insansiz-platformlar", IconClass = "bi bi-robot", SortOrder = 6, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Uav, Name = "İHA (UAV)", Slug = "iha-uav", ParentCategoryId = CategoryConstants.InsansizPlatformlar, ModelTypeName = "DefenceDB.EL.Models.Products.UAV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Usv, Name = "İDA (USV)", Slug = "ida-usv", ParentCategoryId = CategoryConstants.InsansizPlatformlar, ModelTypeName = "DefenceDB.EL.Models.Products.USV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Ugv, Name = "İKA (UGV)", Slug = "ika-ugv", ParentCategoryId = CategoryConstants.InsansizPlatformlar, ModelTypeName = "DefenceDB.EL.Models.Products.UGV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.KamikazeUav, Name = "Kamikaze İHA", Slug = "kamikaze-iha", ParentCategoryId = CategoryConstants.InsansizPlatformlar, ModelTypeName = "DefenceDB.EL.Models.Products.KamikazeUAV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.KamikazeUsv, Name = "Kamikaze İDA", Slug = "kamikaze-ida", ParentCategoryId = CategoryConstants.InsansizPlatformlar, ModelTypeName = "DefenceDB.EL.Models.Products.KamikazeUSV", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // Motor ve Güç Sistemleri (30) ve Alt Kategorileri (31-38)
            new Category { Id = CategoryConstants.MotorVeGucSistemleri, Name = "Motor ve Güç Sistemleri", Slug = "motor-ve-guc-sistemleri", IconClass = "bi bi-gear-wide-connected", SortOrder = 7, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.TurbofanMotorlar, Name = "Turbofan Motorlar", Slug = "turbofan-motorlar", ParentCategoryId = CategoryConstants.MotorVeGucSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.TurbofanEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.PistonluMotorlar, Name = "Pistonlu/İçten Yanmalı Motorlar", Slug = "pistonlu-motorlar", ParentCategoryId = CategoryConstants.MotorVeGucSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.PistonEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.RoketMotorlari, Name = "Roket Motorları", Slug = "roket-motorlari", ParentCategoryId = CategoryConstants.MotorVeGucSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.RocketMotor", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.ElektrikVeNukleerGuc, Name = "Elektrik ve Nükleer Güç", Slug = "elektrik-ve-nukleer-guc", ParentCategoryId = CategoryConstants.MotorVeGucSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.ElectricNuclearPower", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.TurbojetMotorlar, Name = "Turbojet Motorlar", Slug = "turbojet-motorlar", ParentCategoryId = CategoryConstants.MotorVeGucSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.TurbojetEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.TurbopropMotorlar, Name = "Turboprop Motorlar", Slug = "turboprop-motorlar", ParentCategoryId = CategoryConstants.MotorVeGucSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.TurbopropEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.DenizGazTurbinleri, Name = "Deniz Gaz Türbinleri", Slug = "deniz-gaz-turbinleri", ParentCategoryId = CategoryConstants.MotorVeGucSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.MarineGasTurbine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.TurboshaftMotorlar, Name = "Turboshaft Motorlar", Slug = "turboshaft-motorlar", ParentCategoryId = CategoryConstants.MotorVeGucSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.TurboshaftEngine", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            
            // Hava Savunma Sistemleri (39) ve Alt Kategorileri (44-47)
            new Category { Id = CategoryConstants.HavaSavunmaSistemleri, Name = "Hava Savunma Sistemleri", Slug = "hava-savunma-sistemleri", IconClass = "bi bi-shield-fill-check", SortOrder = 8, ModelTypeName = "DefenceDB.EL.Models.Products.AirDefenseSystem", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Manpads, Name = "Taşınabilir Hava Savunma Sistemleri (MANPADS)", Slug = "manpads", ParentCategoryId = CategoryConstants.HavaSavunmaSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.AirDefenseSystem", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Ciws, Name = "Yakın Savunma Silah Sistemleri (CIWS)", Slug = "ciws", ParentCategoryId = CategoryConstants.HavaSavunmaSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.AirDefenseSystem", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Spaag, Name = "Kundağı Motorlu Uçaksavar Topları (SPAAG)", Slug = "spaag", ParentCategoryId = CategoryConstants.HavaSavunmaSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.AirDefenseSystem", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.HavaVeFuzeSavunma, Name = "Hava ve Füze Savunma Sistemleri", Slug = "hava-ve-fuze-savunma-sistemleri", ParentCategoryId = CategoryConstants.HavaSavunmaSistemleri, ModelTypeName = "DefenceDB.EL.Models.Products.AirDefenseSystem", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Piyade Silahları (54) ve Alt Kategorileri (55-58)
            new Category { Id = CategoryConstants.PiyadeSilahlari, Name = "Piyade Silahları", Slug = "piyade-silahlari", IconClass = "bi bi-crosshair", SortOrder = 9, ModelTypeName = null, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.Tabancalar, Name = "Tabancalar", Slug = "tabancalar", ParentCategoryId = CategoryConstants.PiyadeSilahlari, ModelTypeName = "DefenceDB.EL.Models.Products.InfantryWeapon", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.PiyadeTufekleri, Name = "Piyade Tüfekleri", Slug = "piyade-tufekleri", ParentCategoryId = CategoryConstants.PiyadeSilahlari, ModelTypeName = "DefenceDB.EL.Models.Products.InfantryWeapon", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.MakineliTufekler, Name = "Makineli Tüfekler", Slug = "makineli-tufekler", ParentCategoryId = CategoryConstants.PiyadeSilahlari, ModelTypeName = "DefenceDB.EL.Models.Products.InfantryWeapon", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = CategoryConstants.KeskinNisanciTufekleri, Name = "Keskin Nişancı Tüfekleri", Slug = "keskin-nisanci-tufekleri", ParentCategoryId = CategoryConstants.PiyadeSilahlari, ModelTypeName = "DefenceDB.EL.Models.Products.InfantryWeapon", CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
