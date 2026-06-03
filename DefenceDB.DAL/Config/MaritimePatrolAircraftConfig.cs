using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;
using DefenceDB.EL.Constants;

namespace DefenceDB.DAL.Config;

public class MaritimePatrolAircraftConfig : IEntityTypeConfiguration<MaritimePatrolAircraft>
{
    public void Configure(EntityTypeBuilder<MaritimePatrolAircraft> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new MaritimePatrolAircraft
            {
                Id = 503,
                CategoryId = CategoryConstants.DenizKarakolUcaklari,
                Name = "P-8A Poseidon",
                Slug = "p-8a-poseidon",
                Manufacturer = "Boeing",
                Country = "ABD",
                Description = "Denizaltı savunma harbi, su üstü harbi ve istihbarat uçağı.",
                EnduranceHours = 10.5,
                SonarType = "AN/APY-10",
                HasTorpedoTubes = true,
                CreatedAt = now,
                IsShowcase = true
            },
            new MaritimePatrolAircraft
            {
                Id = 504,
                CategoryId = CategoryConstants.DenizKarakolUcaklari,
                Name = "ATR 72 Meltem III",
                Slug = "atr-72-meltem-iii",
                Manufacturer = "Alenia Aermacchi / TUSAŞ",
                Country = "Türkiye / İtalya",
                Description = "Denizaltı savunma harbi ve deniz karakol uçağı.",
                EnduranceHours = 9.0,
                SonarType = "AMASCOS",
                HasTorpedoTubes = true,
                CreatedAt = now,
                IsShowcase = true
            }
        );
    }
}
