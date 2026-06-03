using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;
using DefenceDB.EL.Constants;

namespace DefenceDB.DAL.Config;

public class AirToAirMissileConfig : IEntityTypeConfiguration<AirToAirMissile>
{
    public void Configure(EntityTypeBuilder<AirToAirMissile> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new AirToAirMissile { Id = 12, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "AIM-9 Sidewinder", Slug = "AIM-9 Sidewinder".ToSlug(), Manufacturer = "Raytheon", Country = "ABD", GuidanceType = "Infrared (IR)", MaxSpeedMach = 2.5, FoxCode = "Fox 2", CreatedAt = now },
            new AirToAirMissile { Id = 13, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "AIM-120 AMRAAM", Slug = "AIM-120 AMRAAM".ToSlug(), Manufacturer = "Raytheon", Country = "ABD", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.0, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 14, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "Meteor", Slug = "Meteor".ToSlug(), Manufacturer = "MBDA", Country = "Avrupa Birliği", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.0, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 15, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "IRIS-T", Slug = "IRIS-T".ToSlug(), Manufacturer = "Diehl Defence", Country = "Almanya", GuidanceType = "Infrared (IR)", MaxSpeedMach = 3.0, FoxCode = "Fox 2", CreatedAt = now },
            new AirToAirMissile { Id = 16, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "GÖKDOĞAN", Slug = "GÖKDOĞAN".ToSlug(), Manufacturer = "TÜBİTAK SAGE", Country = "Türkiye", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.0, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 17, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "BOZDOĞAN", Slug = "BOZDOĞAN".ToSlug(), Manufacturer = "TÜBİTAK SAGE", Country = "Türkiye", GuidanceType = "Infrared (IR)", MaxSpeedMach = 4.0, FoxCode = "Fox 2", CreatedAt = now },
            new AirToAirMissile { Id = 18, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "R-77 (AA-12 Adder)", Slug = "R-77 (AA-12 Adder)".ToSlug(), Manufacturer = "Vympel", Country = "Rusya", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.0, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 19, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "R-73 (AA-11 Archer)", Slug = "R-73 (AA-11 Archer)".ToSlug(), Manufacturer = "Vympel", Country = "Rusya", GuidanceType = "Infrared (IR)", MaxSpeedMach = 2.5, FoxCode = "Fox 2", CreatedAt = now },
            new AirToAirMissile { Id = 20, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "PL-15", Slug = "PL-15".ToSlug(), Manufacturer = "CASC", Country = "Çin", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.5, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 21, CategoryId = CategoryConstants.HavaHavaFuzeleri, Name = "MICA", Slug = "MICA".ToSlug(), Manufacturer = "MBDA", Country = "Fransa", GuidanceType = "IR/RF", MaxSpeedMach = 3.0, FoxCode = "Fox 2 / Fox 3", CreatedAt = now }
        );
    }
}
