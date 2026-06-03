using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;
using DefenceDB.EL.Constants;

namespace DefenceDB.DAL.Config;

public class FighterAircraftConfig : IEntityTypeConfiguration<FighterAircraft>
{
    public void Configure(EntityTypeBuilder<FighterAircraft> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new FighterAircraft { Id = 1, CategoryId = CategoryConstants.AvciUcaklari, Name = "F-16 Fighting Falcon", Slug = "F-16 Fighting Falcon".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", Description = "Çok amaçlı 4. nesil savaş uçağı.", Generation = "4", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 550, CreatedAt = now, IsShowcase = true },
            new FighterAircraft { Id = 2, CategoryId = CategoryConstants.AvciUcaklari, Name = "F-35 Lightning II", Slug = "F-35 Lightning II".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", Description = "5. nesil çok amaçlı hayalet savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 1090, CreatedAt = now, IsShowcase = true },
            new FighterAircraft { Id = 3, CategoryId = CategoryConstants.AvciUcaklari, Name = "F-22 Raptor", Slug = "F-22 Raptor".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", Description = "Hava üstünlüğü sağlayan 5. nesil savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 850, CreatedAt = now },
            new FighterAircraft { Id = 4, CategoryId = CategoryConstants.AvciUcaklari, Name = "KAAN", Slug = "KAAN".ToSlug(), Manufacturer = "TUSAŞ", Country = "Türkiye", Description = "Milli Muharip Uçak (MMU). 5. nesil çok rollü savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 1100, CreatedAt = now, IsShowcase = true },
            new FighterAircraft { Id = 5, CategoryId = CategoryConstants.AvciUcaklari, Name = "Eurofighter Typhoon", Slug = "Eurofighter Typhoon".ToSlug(), Manufacturer = "Eurofighter Jagdflugzeug", Country = "Avrupa Birliği", Description = "Çift motorlu, delta kanatlı çok rollü savaş uçağı.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 1390, CreatedAt = now },
            new FighterAircraft { Id = 6, CategoryId = CategoryConstants.AvciUcaklari, Name = "Dassault Rafale", Slug = "Dassault Rafale".ToSlug(), Manufacturer = "Dassault Aviation", Country = "Fransa", Description = "Omnirole savaş uçağı.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 1850, CreatedAt = now },
            new FighterAircraft { Id = 7, CategoryId = CategoryConstants.AvciUcaklari, Name = "Su-57", Slug = "Su-57".ToSlug(), Manufacturer = "Sukhoi", Country = "Rusya", Description = "Rusya'nın 5. nesil savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 1500, CreatedAt = now },
            new FighterAircraft { Id = 8, CategoryId = CategoryConstants.AvciUcaklari, Name = "Su-35", Slug = "Su-35".ToSlug(), Manufacturer = "Sukhoi", Country = "Rusya", Description = "Gelişmiş 4++ nesil çok amaçlı savaş uçağı.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 1600, CreatedAt = now },
            new FighterAircraft { Id = 9, CategoryId = CategoryConstants.AvciUcaklari, Name = "J-20 Mighty Dragon", Slug = "J-20 Mighty Dragon".ToSlug(), Manufacturer = "Chengdu", Country = "Çin", Description = "Çin'in 5. nesil ağır savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 2000, CreatedAt = now },
            new FighterAircraft { Id = 10, CategoryId = CategoryConstants.AvciUcaklari, Name = "JAS 39 Gripen", Slug = "JAS 39 Gripen".ToSlug(), Manufacturer = "Saab", Country = "İsveç", Description = "Hafif, tek motorlu çok amaçlı uçak.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 800, CreatedAt = now },
            new FighterAircraft { Id = 11, CategoryId = CategoryConstants.AvciUcaklari, Name = "F-15EX Eagle II", Slug = "F-15EX Eagle II".ToSlug(), Manufacturer = "Boeing", Country = "ABD", Description = "F-15'in modernize edilmiş en gelişmiş versiyonu.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 1270, CreatedAt = now }
        );
    }
}
