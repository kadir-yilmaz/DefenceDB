using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;

namespace DefenceDB.DAL.Config;

public class UAVConfig : IEntityTypeConfiguration<UAV>
{
    public void Configure(EntityTypeBuilder<UAV> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new UAV { Id = 201, CategoryId = 24, Name = "Bayraktar TB2", Slug = "Bayraktar TB2".ToSlug(), Manufacturer = "Baykar", Country = "Türkiye", EnduranceHours = 27, MaxAltitudeFeet = 25000, PayloadCapacityKg = 150, WingSpanMeters = 12, CruisingSpeedKmh = 130, CreatedAt = now, IsShowcase = true },
            new UAV { Id = 202, CategoryId = 24, Name = "Bayraktar TB3", Slug = "Bayraktar TB3".ToSlug(), Manufacturer = "Baykar", Country = "Türkiye", EnduranceHours = 24, MaxAltitudeFeet = 25000, PayloadCapacityKg = 280, WingSpanMeters = 14, CruisingSpeedKmh = 160, CreatedAt = now, IsShowcase = true },
            new UAV { Id = 203, CategoryId = 24, Name = "Bayraktar Akıncı", Slug = "Bayraktar Akıncı".ToSlug(), Manufacturer = "Baykar", Country = "Türkiye", EnduranceHours = 24, MaxAltitudeFeet = 40000, PayloadCapacityKg = 1500, WingSpanMeters = 20, CruisingSpeedKmh = 277, CreatedAt = now, IsShowcase = true },
            new UAV { Id = 204, CategoryId = 24, Name = "Bayraktar Kızılelma", Slug = "Bayraktar Kızılelma".ToSlug(), Manufacturer = "Baykar", Country = "Türkiye", EnduranceHours = 5, MaxAltitudeFeet = 45000, PayloadCapacityKg = 1500, WingSpanMeters = 10, CruisingSpeedKmh = 735, CreatedAt = now, IsShowcase = true },
            new UAV { Id = 205, CategoryId = 24, Name = "MQ-9 Reaper", Slug = "MQ-9 Reaper".ToSlug(), Manufacturer = "General Atomics", Country = "ABD", EnduranceHours = 27, MaxAltitudeFeet = 50000, PayloadCapacityKg = 1700, WingSpanMeters = 20, CruisingSpeedKmh = 313, CreatedAt = now },
            new UAV { Id = 206, CategoryId = 24, Name = "RQ-4 Global Hawk", Slug = "RQ-4 Global Hawk".ToSlug(), Manufacturer = "Northrop Grumman", Country = "ABD", EnduranceHours = 34, MaxAltitudeFeet = 60000, PayloadCapacityKg = 1360, WingSpanMeters = 39.9, CruisingSpeedKmh = 575, CreatedAt = now },
            new UAV { Id = 207, CategoryId = 24, Name = "Wing Loong II", Slug = "Wing Loong II".ToSlug(), Manufacturer = "Chengdu", Country = "Çin", EnduranceHours = 32, MaxAltitudeFeet = 32500, PayloadCapacityKg = 480, WingSpanMeters = 20.5, CruisingSpeedKmh = 370, CreatedAt = now },
            new UAV { Id = 208, CategoryId = 24, Name = "CH-4 Rainbow", Slug = "CH-4 Rainbow".ToSlug(), Manufacturer = "CASC", Country = "Çin", EnduranceHours = 40, MaxAltitudeFeet = 23600, PayloadCapacityKg = 345, WingSpanMeters = 18, CruisingSpeedKmh = 235, CreatedAt = now },
            new UAV { Id = 209, CategoryId = 24, Name = "Orion", Slug = "Orion".ToSlug(), Manufacturer = "Kronshtadt Group", Country = "Rusya", EnduranceHours = 24, MaxAltitudeFeet = 24600, PayloadCapacityKg = 250, WingSpanMeters = 16.3, CruisingSpeedKmh = 120, CreatedAt = now },
            new UAV { Id = 210, CategoryId = 24, Name = "S-70 Okhotnik", Slug = "S-70 Okhotnik".ToSlug(), Manufacturer = "Sukhoi", Country = "Rusya", EnduranceHours = 12, MaxAltitudeFeet = 34400, PayloadCapacityKg = 2800, WingSpanMeters = 20, CruisingSpeedKmh = 1000, CreatedAt = now },
            new UAV { Id = 211, CategoryId = 24, Name = "Anka-S", Slug = "Anka-S".ToSlug(), Manufacturer = "TUSAŞ", Country = "Türkiye", EnduranceHours = 30, MaxAltitudeFeet = 30000, PayloadCapacityKg = 350, WingSpanMeters = 17.5, CruisingSpeedKmh = 200, CreatedAt = now },
            new UAV { Id = 212, CategoryId = 24, Name = "Aksungur", Slug = "Aksungur".ToSlug(), Manufacturer = "TUSAŞ", Country = "Türkiye", EnduranceHours = 50, MaxAltitudeFeet = 40000, PayloadCapacityKg = 750, WingSpanMeters = 24, CruisingSpeedKmh = 250, CreatedAt = now, IsShowcase = true },
            new UAV { Id = 213, CategoryId = 24, Name = "Anka-3", Slug = "Anka-3".ToSlug(), Manufacturer = "TUSAŞ", Country = "Türkiye", EnduranceHours = 10, MaxAltitudeFeet = 40000, PayloadCapacityKg = 1200, WingSpanMeters = 12, CruisingSpeedKmh = 800, CreatedAt = now, IsShowcase = true }
        );
    }
}
