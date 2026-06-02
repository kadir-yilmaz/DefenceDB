using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;

namespace DefenceDB.DAL.Config;

public class AwacsAircraftConfig : IEntityTypeConfiguration<AwacsAircraft>
{
    public void Configure(EntityTypeBuilder<AwacsAircraft> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new AwacsAircraft
            {
                Id = 505,
                CategoryId = 43,
                Name = "E-7T Barış Kartalı",
                Slug = "e-7t-baris-kartali",
                Manufacturer = "Boeing / TUSAŞ",
                Country = "Türkiye / ABD",
                Description = "Havadan erken ihbar ve kontrol uçağı (HİK).",
                RadarType = "MESA (Çok Rollü Elektronik Taramalı Dizi)",
                DetectionRangeKm = 400,
                MaxTrackedTargets = 180,
                CreatedAt = now,
                IsShowcase = true
            }
        );
    }
}
