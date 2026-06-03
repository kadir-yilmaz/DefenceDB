using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;
using DefenceDB.EL.Constants;

namespace DefenceDB.DAL.Config;

public class InfantryWeaponConfig : IEntityTypeConfiguration<InfantryWeapon>
{
    public void Configure(EntityTypeBuilder<InfantryWeapon> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new InfantryWeapon
            {
                Id = 601,
                CategoryId = CategoryConstants.Tabancalar,
                Name = "SAR 9",
                Slug = "sar-9",
                Manufacturer = "Sarsılmaz",
                Country = "Türkiye",
                Description = "Emniyet Genel Müdürlüğü ve Türk Silahlı Kuvvetleri'nin ana hizmet tabancası.",
                Caliber = "9x19mm Parabellum",
                EffectiveRangeMeters = 50,
                WeightKg = 0.79,
                MagazineCapacity = 15,
                CreatedAt = now,
                IsShowcase = true
            },
            new InfantryWeapon
            {
                Id = 602,
                CategoryId = CategoryConstants.PiyadeTufekleri,
                Name = "MPT-76",
                Slug = "mpt-76",
                Manufacturer = "MKE / Kale Kalıp / Sarsılmaz",
                Country = "Türkiye",
                Description = "Tamamen milli imkanlarla geliştirilen TSK'nın ana piyade tüfeği.",
                Caliber = "7.62x51mm NATO",
                EffectiveRangeMeters = 600,
                RateOfFireRpm = 700,
                WeightKg = 4.1,
                MagazineCapacity = 20,
                CreatedAt = now,
                IsShowcase = true
            },
            new InfantryWeapon
            {
                Id = 603,
                CategoryId = CategoryConstants.PiyadeTufekleri,
                Name = "MPT-55",
                Slug = "mpt-55",
                Manufacturer = "MKE",
                Country = "Türkiye",
                Description = "Özel kuvvetler ve emniyet birimleri için kısa namlulu ve hafif milli piyade tüfeği.",
                Caliber = "5.56x45mm NATO",
                EffectiveRangeMeters = 400,
                RateOfFireRpm = 800,
                WeightKg = 3.3,
                MagazineCapacity = 30,
                CreatedAt = now,
                IsShowcase = false
            },
            new InfantryWeapon
            {
                Id = 604,
                CategoryId = CategoryConstants.MakineliTufekler,
                Name = "SAR 762 MT",
                Slug = "sar-762-mt",
                Manufacturer = "Sarsılmaz",
                Country = "Türkiye",
                Description = "Araç üstü ve piyade kullanımı için geliştirilmiş yerli makineli tüfek.",
                Caliber = "7.62x51mm NATO",
                EffectiveRangeMeters = 1200,
                RateOfFireRpm = 850,
                WeightKg = 12.0,
                MagazineCapacity = 100,
                CreatedAt = now,
                IsShowcase = true
            },
            new InfantryWeapon
            {
                Id = 605,
                CategoryId = CategoryConstants.KeskinNisanciTufekleri,
                Name = "KNT-76",
                Slug = "knt-76",
                Manufacturer = "MKE",
                Country = "Türkiye",
                Description = "MPT-76 platformu üzerinden geliştirilen yarı otomatik manga tipi keskin nişancı tüfeği.",
                Caliber = "7.62x51mm NATO",
                EffectiveRangeMeters = 800,
                WeightKg = 4.7,
                MagazineCapacity = 20,
                CreatedAt = now,
                IsShowcase = true
            }
        );
    }
}
