using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;

namespace DefenceDB.DAL.Config;

public class AirDefenseSystemConfig : IEntityTypeConfiguration<AirDefenseSystem>
{
    public void Configure(EntityTypeBuilder<AirDefenseSystem> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new AirDefenseSystem 
            { 
                Id = 401, 
                CategoryId = 39, 
                Name = "S-400 Triumf", 
                Slug = "s-400-triumf", 
                Manufacturer = "Almaz-Antey", 
                Country = "Rusya", 
                Description = "Rusya yapımı orta ve uzun menzilli uçaksavar ve füze savunma sistemi.", 
                MaxSearchRangeKm = 600, 
                MaxTrackingRangeKm = 400, 
                MaxEngagementAltitudeFt = 100000, 
                MaxTrackedTargets = 300, 
                MissilesPerLauncher = 4, 
                HasAntiBallisticCapability = true, 
                CreatedAt = now, 
                IsShowcase = true 
            },
            new AirDefenseSystem 
            { 
                Id = 402, 
                CategoryId = 39, 
                Name = "S-500 Prometey", 
                Slug = "s-500-prometey", 
                Manufacturer = "Almaz-Antey", 
                Country = "Rusya", 
                Description = "Uzay ve kıtalararası balistik füze savunması odaklı yeni nesil uzun menzilli hava savunma sistemi.", 
                MaxSearchRangeKm = 800, 
                MaxTrackingRangeKm = 600, 
                MaxEngagementAltitudeFt = 650000, 
                MaxTrackedTargets = 100, 
                MissilesPerLauncher = 4, 
                HasAntiBallisticCapability = true, 
                CreatedAt = now, 
                IsShowcase = true 
            },
            new AirDefenseSystem 
            { 
                Id = 403, 
                CategoryId = 39, 
                Name = "MIM-104 Patriot (PAC-3)", 
                Slug = "mim-104-patriot-pac-3", 
                Manufacturer = "Raytheon / Lockheed Martin", 
                Country = "ABD", 
                Description = "Amerika Birleşik Devletleri ordusunun ana taktik hava ve balistik füze savunma sistemi.", 
                MaxSearchRangeKm = 150, 
                MaxTrackingRangeKm = 100, 
                MaxEngagementAltitudeFt = 80000, 
                MaxTrackedTargets = 100, 
                MissilesPerLauncher = 16, 
                HasAntiBallisticCapability = true, 
                CreatedAt = now, 
                IsShowcase = true 
            },
            new AirDefenseSystem 
            { 
                Id = 404, 
                CategoryId = 39, 
                Name = "THAAD", 
                Slug = "thaad", 
                Manufacturer = "Lockheed Martin", 
                Country = "ABD", 
                Description = "Atmosfer içi ve atmosfer dışı (Terminal aşamada) kısa, orta ve ara menzilli balistik füzeleri önleme sistemi.", 
                MaxSearchRangeKm = 1000, 
                MaxTrackingRangeKm = 800, 
                MaxEngagementAltitudeFt = 490000, 
                MaxTrackedTargets = 100, 
                MissilesPerLauncher = 8, 
                HasAntiBallisticCapability = true, 
                CreatedAt = now, 
                IsShowcase = true 
            },
            new AirDefenseSystem 
            { 
                Id = 405, 
                CategoryId = 39, 
                Name = "SAMP/T Mamba", 
                Slug = "samp-t-mamba", 
                Manufacturer = "Eurosam", 
                Country = "Fransa", 
                Description = "Aster 30 füzelerini kullanan, uçak ve seyir füzelerine ek olarak balistik füzelere karşı da etkili Avrupa menşeili hava savunma sistemi.", 
                MaxSearchRangeKm = 150, 
                MaxTrackingRangeKm = 100, 
                MaxEngagementAltitudeFt = 65000, 
                MaxTrackedTargets = 100, 
                MissilesPerLauncher = 8, 
                HasAntiBallisticCapability = true, 
                CreatedAt = now, 
                IsShowcase = false 
            },
            new AirDefenseSystem 
            { 
                Id = 406, 
                CategoryId = 39, 
                Name = "HQ-9", 
                Slug = "hq-9", 
                Manufacturer = "CASIC", 
                Country = "Çin", 
                Description = "Çin Halk Kurtuluş Ordusu'nun ana uzun menzilli hava ve füze savunma sistemi.", 
                MaxSearchRangeKm = 250, 
                MaxTrackingRangeKm = 180, 
                MaxEngagementAltitudeFt = 98000, 
                MaxTrackedTargets = 100, 
                MissilesPerLauncher = 4, 
                HasAntiBallisticCapability = true, 
                CreatedAt = now, 
                IsShowcase = false 
            },
            new AirDefenseSystem 
            { 
                Id = 408, 
                CategoryId = 39, 
                Name = "HİSAR-A+", 
                Slug = "hisar-a-plus", 
                Manufacturer = "Aselsan / Roketsan", 
                Country = "Türkiye", 
                Description = "Türkiye tarafından milli imkanlarla geliştirilen, savaş uçakları, helikopterler, seyir füzeleri ve İHA'lara karşı etkili alçak irtifa hava savunma sistemi.", 
                MaxSearchRangeKm = 35, 
                MaxTrackingRangeKm = 25, 
                MaxEngagementAltitudeFt = 26000, 
                MaxTrackedTargets = 60, 
                MissilesPerLauncher = 4, 
                HasAntiBallisticCapability = false, 
                CreatedAt = now, 
                IsShowcase = true 
            },
            new AirDefenseSystem 
            { 
                Id = 409, 
                CategoryId = 39, 
                Name = "HİSAR-O+", 
                Slug = "hisar-o-plus", 
                Manufacturer = "Aselsan / Roketsan", 
                Country = "Türkiye", 
                Description = "Türkiye tarafından yerli imkanlarla tasarlanan ve geliştirilen, savaş uçakları, İHA'lar, seyir füzeleri ve helikopterler gibi hedefleri imha etmek üzere tasarlanmış orta irtifa hava savunma sistemi.", 
                MaxSearchRangeKm = 80, 
                MaxTrackingRangeKm = 60, 
                MaxEngagementAltitudeFt = 49000, 
                MaxTrackedTargets = 60, 
                MissilesPerLauncher = 6, 
                HasAntiBallisticCapability = false, 
                CreatedAt = now, 
                IsShowcase = true 
            },
            new AirDefenseSystem
            {
                Id = 411,
                CategoryId = 39,
                Name = "K-SAM Chunma (Pegasus)",
                Slug = "k-sam-chunma-pegasus",
                Manufacturer = "Hanwha Defense / LIG Nex1",
                Country = "Güney Kore",
                Description = "Güney Kore ordusunun kritik tesislerini ve birliklerini korumak için tasarlanmış mobil alçak irtifa hava savunma sistemi.",
                MaxSearchRangeKm = 20,
                MaxTrackingRangeKm = 16,
                MaxEngagementAltitudeFt = 16000,
                MaxTrackedTargets = 20,
                MissilesPerLauncher = 8,
                HasAntiBallisticCapability = false,
                CreatedAt = now,
                IsShowcase = false
            },
            new AirDefenseSystem
            {
                Id = 412,
                CategoryId = 39,
                Name = "Cheongung I (M-SAM)",
                Slug = "cheongung-i-m-sam",
                Manufacturer = "LIG Nex1 / Hanwha Systems",
                Country = "Güney Kore",
                Description = "Güney Kore yapımı, orta irtifadaki hava tehditlerine (uçaklar ve seyir füzeleri) karşı geliştirilmiş ilk nesil Cheongung hava savunma sistemi.",
                MaxSearchRangeKm = 100,
                MaxTrackingRangeKm = 80,
                MaxEngagementAltitudeFt = 49000,
                MaxTrackedTargets = 40,
                MissilesPerLauncher = 8,
                HasAntiBallisticCapability = false,
                CreatedAt = now,
                IsShowcase = false
            },
            new AirDefenseSystem
            {
                Id = 413,
                CategoryId = 39,
                Name = "L-SAM",
                Slug = "l-sam",
                Manufacturer = "LIG Nex1 / Hanwha Systems",
                Country = "Güney Kore",
                Description = "Güney Kore'nin katmanlı füze savunma kalkanı (KAMD) kapsamında geliştirdiği, üst irtifadaki balistik füzeleri ve hava tehditlerini önleme amaçlı uzun menzilli hava savunma sistemi.",
                MaxSearchRangeKm = 300,
                MaxTrackingRangeKm = 250,
                MaxEngagementAltitudeFt = 190000,
                MaxTrackedTargets = 100,
                MissilesPerLauncher = 6,
                HasAntiBallisticCapability = true,
                CreatedAt = now,
                IsShowcase = true
            },
            new AirDefenseSystem
            {
                Id = 414,
                CategoryId = 39,
                Name = "Cheongung II (M-SAM Block II)",
                Slug = "cheongung-ii-m-sam-block-ii",
                Manufacturer = "LIG Nex1 / Hanwha Systems",
                Country = "Güney Kore",
                Description = "Cheongung I'in geliştirilmiş versiyonu olan orta irtifa hava savunma sistemi. Daha gelişmiş radar, artırılmış menzil ve çoklu hedef takip yeteneği sunar.",
                MaxSearchRangeKm = 150,
                MaxTrackingRangeKm = 120,
                MaxEngagementAltitudeFt = 60000,
                MaxTrackedTargets = 50,
                MissilesPerLauncher = 8,
                HasAntiBallisticCapability = false,
                CreatedAt = now,
                IsShowcase = false
            },
            new AirDefenseSystem
            {
                Id = 415,
                CategoryId = 39,
                Name = "KP-SAM (Shin-Gung)",
                Slug = "kp-sam-shin-gung",
                Manufacturer = "LIG Nex1",
                Country = "Güney Kore",
                Description = "Güney Kore'nin taşınabilir hava savunma sistemi (MANPADS). Helikopterler, İHA'lar ve alçak uçan uçaklara karşı etkili.",
                MaxSearchRangeKm = 8,
                MaxTrackingRangeKm = 6,
                MaxEngagementAltitudeFt = 12000,
                MaxTrackedTargets = 2,
                MissilesPerLauncher = 1,
                HasAntiBallisticCapability = false,
                CreatedAt = now,
                IsShowcase = false
            },
            new AirDefenseSystem
            {
                Id = 416,
                CategoryId = 39,
                Name = "Biho (K30)",
                Slug = "biho-k30",
                Manufacturer = "Doosan DST / S&T Dynamics",
                Country = "Güney Kore",
                Description = "Güney Kore yapımı kendinden tahrikli uçaksavar topu (SPAAG). 30mm çift namlulu top ve kısa menzilli füzelerle donatılmıştır.",
                MaxSearchRangeKm = 15,
                MaxTrackingRangeKm = 12,
                MaxEngagementAltitudeFt = 10000,
                MaxTrackedTargets = 10,
                MissilesPerLauncher = 4,
                HasAntiBallisticCapability = false,
                CreatedAt = now,
                IsShowcase = false
            },
            new AirDefenseSystem
            {
                Id = 417,
                CategoryId = 39,
                Name = "Cheonma (K-SAM Block II)",
                Slug = "cheonma-k-sam-block-ii",
                Manufacturer = "Hanwha Defense / LIG Nex1",
                Country = "Güney Kore",
                Description = "K-SAM Chunma'nın geliştirilmiş versiyonu olan alçak irtifa hava savunma sistemi. Daha modern radar ve geliştirilmiş füze teknolojisi kullanır.",
                MaxSearchRangeKm = 25,
                MaxTrackingRangeKm = 20,
                MaxEngagementAltitudeFt = 20000,
                MaxTrackedTargets = 25,
                MissilesPerLauncher = 8,
                HasAntiBallisticCapability = false,
                CreatedAt = now,
                IsShowcase = false
            },
            new AirDefenseSystem
            {
                Id = 418,
                CategoryId = 39,
                Name = "MIM-104 Patriot (PAC-2)",
                Slug = "mim-104-patriot-pac-2",
                Manufacturer = "Raytheon / Lockheed Martin",
                Country = "ABD",
                Description = "Patriot sisteminin PAC-2 konfigürasyonu. Öncelikle geleneksel hava tehditlerine (uçaklar, helikopterler, seyir füzeleri) karşı etkili, sınırlı balistik füze savunma yeteneği vardır.",
                MaxSearchRangeKm = 100,
                MaxTrackingRangeKm = 90,
                MaxEngagementAltitudeFt = 60000,
                MaxTrackedTargets = 100,
                MissilesPerLauncher = 4,
                HasAntiBallisticCapability = false,
                CreatedAt = now,
                IsShowcase = false
            }
        );
    }
}
