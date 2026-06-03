using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;
using DefenceDB.EL.Constants;

namespace DefenceDB.DAL.Config;

public class AntiShipMissileConfig : IEntityTypeConfiguration<AntiShipMissile>
{
    public void Configure(EntityTypeBuilder<AntiShipMissile> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new AntiShipMissile { Id = 31, CategoryId = CategoryConstants.AntiGemiFuzeleri, Name = "Harpoon", Slug = "Harpoon".ToSlug(), Manufacturer = "Boeing", Country = "ABD", SeaSkimming = true, SpeedClass = "Subsonic", RangeKm = 140, CreatedAt = now },
            new AntiShipMissile { Id = 32, CategoryId = CategoryConstants.AntiGemiFuzeleri, Name = "ATMACA", Slug = "ATMACA".ToSlug(), Manufacturer = "Roketsan", Country = "Türkiye", SeaSkimming = true, SpeedClass = "Subsonic", RangeKm = 220, CreatedAt = now, IsShowcase = true }
        );
    }
}

public class BallisticMissileConfig : IEntityTypeConfiguration<BallisticMissile>
{
    public void Configure(EntityTypeBuilder<BallisticMissile> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new BallisticMissile { Id = 33, CategoryId = CategoryConstants.BalistikFuzeler, Name = "Trident II D5", Slug = "Trident II D5".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", RangeKm = 12000, PayloadKg = 2800, IsNuclearCapable = true, HasMirv = true, CreatedAt = now },
            new BallisticMissile { Id = 34, CategoryId = CategoryConstants.BalistikFuzeler, Name = "TAYFUN", Slug = "TAYFUN".ToSlug(), Manufacturer = "Roketsan", Country = "Türkiye", RangeKm = 560, PayloadKg = 500, IsNuclearCapable = false, HasMirv = false, CreatedAt = now }
        );
    }
}

public class CruiseMissileConfig : IEntityTypeConfiguration<CruiseMissile>
{
    public void Configure(EntityTypeBuilder<CruiseMissile> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new CruiseMissile { Id = 39, CategoryId = CategoryConstants.SeyirFuzeleri, Name = "Tomahawk", Slug = "Tomahawk".ToSlug(), Manufacturer = "Raytheon", Country = "ABD", RangeKm = 1600, CepMeters = 10, CreatedAt = now },
            new CruiseMissile { Id = 40, CategoryId = CategoryConstants.SeyirFuzeleri, Name = "SOM", Slug = "SOM".ToSlug(), Manufacturer = "TÜBİTAK SAGE / Roketsan", Country = "Türkiye", RangeKm = 250, CepMeters = 5, CreatedAt = now }
        );
    }
}
