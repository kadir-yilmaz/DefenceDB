using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;

namespace DefenceDB.DAL.Config;

public class TankConfig : IEntityTypeConfiguration<Tank>
{
    public void Configure(EntityTypeBuilder<Tank> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new Tank { Id = 101, CategoryId = 22, Name = "Leopard 2A7", Slug = "Leopard 2A7".ToSlug(), Manufacturer = "KMW / Rheinmetall", Country = "Almanya", EngineHorsePower = 1500, MainGunCaliberMm = 120, WeightTons = 66.5, CrewCount = 4, HasAutoloader = false, YearIntroduced = 2014, CreatedAt = now, IsShowcase = true },
            new Tank { Id = 102, CategoryId = 22, Name = "K2 Black Panther", Slug = "K2 Black Panther".ToSlug(), Manufacturer = "Hyundai Rotem", Country = "Güney Kore", EngineHorsePower = 1500, MainGunCaliberMm = 120, WeightTons = 55.0, CrewCount = 3, HasAutoloader = true, YearIntroduced = 2014, CreatedAt = now, IsShowcase = true },
            new Tank { Id = 103, CategoryId = 22, Name = "Altay", Slug = "Altay".ToSlug(), Manufacturer = "BMC / Otokar", Country = "Türkiye", EngineHorsePower = 1500, MainGunCaliberMm = 120, WeightTons = 65.0, CrewCount = 4, HasAutoloader = false, YearIntroduced = 2025, CreatedAt = now, IsShowcase = true },
            new Tank { Id = 104, CategoryId = 22, Name = "M1A2 SEPv3 Abrams", Slug = "M1A2 Abrams".ToSlug(), Manufacturer = "General Dynamics", Country = "ABD", EngineHorsePower = 1500, MainGunCaliberMm = 120, WeightTons = 66.8, CrewCount = 4, HasAutoloader = false, YearIntroduced = 2020, CreatedAt = now },
            new Tank { Id = 105, CategoryId = 22, Name = "T-72B3", Slug = "T-72B3".ToSlug(), Manufacturer = "Uralvagonzavod", Country = "Rusya", EngineHorsePower = 1130, MainGunCaliberMm = 125, WeightTons = 46.0, CrewCount = 3, HasAutoloader = true, YearIntroduced = 2013, CreatedAt = now },
            new Tank { Id = 106, CategoryId = 22, Name = "T-80BVM", Slug = "T-80BVM".ToSlug(), Manufacturer = "Omsktransmash", Country = "Rusya", EngineHorsePower = 1250, MainGunCaliberMm = 125, WeightTons = 46.0, CrewCount = 3, HasAutoloader = true, YearIntroduced = 2017, CreatedAt = now },
            new Tank { Id = 107, CategoryId = 22, Name = "T-90M Proryv", Slug = "T-90M Proryv".ToSlug(), Manufacturer = "Uralvagonzavod", Country = "Rusya", EngineHorsePower = 1130, MainGunCaliberMm = 125, WeightTons = 48.0, CrewCount = 3, HasAutoloader = true, YearIntroduced = 2020, CreatedAt = now },
            new Tank { Id = 108, CategoryId = 22, Name = "Challenger 2", Slug = "Challenger 2".ToSlug(), Manufacturer = "BAE Systems", Country = "Birleşik Krallık", EngineHorsePower = 1200, MainGunCaliberMm = 120, WeightTons = 64.0, CrewCount = 4, HasAutoloader = false, YearIntroduced = 1998, CreatedAt = now },
            new Tank { Id = 109, CategoryId = 22, Name = "Merkava Mk.4", Slug = "Merkava Mk4".ToSlug(), Manufacturer = "MANTAK", Country = "İsrail", EngineHorsePower = 1500, MainGunCaliberMm = 120, WeightTons = 65.0, CrewCount = 4, HasAutoloader = false, YearIntroduced = 2004, CreatedAt = now },
            new Tank { Id = 110, CategoryId = 22, Name = "Leclerc", Slug = "Leclerc".ToSlug(), Manufacturer = "Nexter", Country = "Fransa", EngineHorsePower = 1500, MainGunCaliberMm = 120, WeightTons = 57.4, CrewCount = 3, HasAutoloader = true, YearIntroduced = 1992, CreatedAt = now },
            new Tank { Id = 111, CategoryId = 22, Name = "Type 10", Slug = "Type 10".ToSlug(), Manufacturer = "Mitsubishi Heavy Industries", Country = "Japonya", EngineHorsePower = 1200, MainGunCaliberMm = 120, WeightTons = 44.0, CrewCount = 3, HasAutoloader = true, YearIntroduced = 2012, CreatedAt = now },
            new Tank { Id = 112, CategoryId = 22, Name = "C1 Ariete", Slug = "C1 Ariete".ToSlug(), Manufacturer = "Iveco-Oto Melara", Country = "İtalya", EngineHorsePower = 1247, MainGunCaliberMm = 120, WeightTons = 54.0, CrewCount = 4, HasAutoloader = false, YearIntroduced = 1995, CreatedAt = now }
        );
    }
}
