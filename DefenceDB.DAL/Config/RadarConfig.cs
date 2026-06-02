using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;

namespace DefenceDB.DAL.Config;

public class AirborneRadarConfig : IEntityTypeConfiguration<AirborneRadar>
{
    public void Configure(EntityTypeBuilder<AirborneRadar> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new AirborneRadar { Id = 22, CategoryId = 20, Name = "AN/APG-81", Slug = "AN/APG-81".ToSlug(), Manufacturer = "Northrop Grumman", Country = "ABD", RadarType = "AESA", MaxRangeKm = 160, TargetTrackingCapacity = 23, CreatedAt = now },
            new AirborneRadar { Id = 23, CategoryId = 20, Name = "AN/APG-77", Slug = "AN/APG-77".ToSlug(), Manufacturer = "Northrop Grumman", Country = "ABD", RadarType = "AESA", MaxRangeKm = 240, TargetTrackingCapacity = 100, CreatedAt = now },
            new AirborneRadar { Id = 24, CategoryId = 20, Name = "Captor-E", Slug = "Captor-E".ToSlug(), Manufacturer = "Euroradar", Country = "Avrupa Birliği", RadarType = "AESA", MaxRangeKm = 200, TargetTrackingCapacity = 60, CreatedAt = now },
            new AirborneRadar { Id = 25, CategoryId = 20, Name = "Irbis-E", Slug = "Irbis-E".ToSlug(), Manufacturer = "NIIP", Country = "Rusya", RadarType = "PESA", MaxRangeKm = 400, TargetTrackingCapacity = 30, CreatedAt = now },
            new AirborneRadar { Id = 26, CategoryId = 20, Name = "MURAD", Slug = "MURAD".ToSlug(), Manufacturer = "Aselsan", Country = "Türkiye", RadarType = "AESA", MaxRangeKm = 150, TargetTrackingCapacity = 40, CreatedAt = now }
        );
    }
}

public class AirDefenseRadarConfig : IEntityTypeConfiguration<AirDefenseRadar>
{
    public void Configure(EntityTypeBuilder<AirDefenseRadar> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new AirDefenseRadar { Id = 27, CategoryId = 19, Name = "AN/SPY-1", Slug = "AN/SPY-1".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", RadarType = "PESA", MaxRangeKm = 310, TargetTrackingCapacity = 100, CreatedAt = now },
            new AirDefenseRadar { Id = 28, CategoryId = 19, Name = "Ers-Int (Erken İhbar)", Slug = "Ers-Int (Erken İhbar)".ToSlug(), Manufacturer = "Aselsan", Country = "Türkiye", RadarType = "AESA", MaxRangeKm = 600, TargetTrackingCapacity = 200, CreatedAt = now },
            new AirDefenseRadar { Id = 29, CategoryId = 19, Name = "Patriot AN/MPQ-65", Slug = "Patriot AN/MPQ-65".ToSlug(), Manufacturer = "Raytheon", Country = "ABD", RadarType = "AESA", MaxRangeKm = 150, TargetTrackingCapacity = 100, CreatedAt = now },
            new AirDefenseRadar { Id = 30, CategoryId = 19, Name = "S-400 91N6E", Slug = "S-400 91N6E".ToSlug(), Manufacturer = "Almaz-Antey", Country = "Rusya", RadarType = "AESA", MaxRangeKm = 600, TargetTrackingCapacity = 300, CreatedAt = now }
        );
    }
}
