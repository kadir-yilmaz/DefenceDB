using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;
using DefenceDB.EL.Constants;

namespace DefenceDB.DAL.Config;

public class AirSojAircraftConfig : IEntityTypeConfiguration<AirSojAircraft>
{
    public void Configure(EntityTypeBuilder<AirSojAircraft> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new AirSojAircraft
            {
                Id = 501,
                CategoryId = CategoryConstants.HavaSojUcaklari,
                Name = "HAVA SOJ (Global 6000)",
                Slug = "hava-soj-global-6000",
                Manufacturer = "TUSAŞ / Bombardier",
                Country = "Türkiye",
                Description = "Gelişmiş hava stand-off jammer karıştırma uçağı.",
                JammerType = "Stand-off Jammer (SOJ)",
                FrequencyRange = "Multi-band HF/VHF/UHF/SHF",
                MaxRangeKm = 3000,
                CreatedAt = now,
                IsShowcase = true
            }
        );
    }
}
