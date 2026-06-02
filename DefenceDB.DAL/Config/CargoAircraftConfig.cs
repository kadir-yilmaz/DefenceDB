using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;

namespace DefenceDB.DAL.Config;

public class CargoAircraftConfig : IEntityTypeConfiguration<CargoAircraft>
{
    public void Configure(EntityTypeBuilder<CargoAircraft> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new CargoAircraft
            {
                Id = 502,
                CategoryId = 41,
                Name = "A400M Atlas",
                Slug = "a400m-atlas",
                Manufacturer = "Airbus Defence and Space",
                Country = "Avrupa Birliği",
                Description = "Dört motorlu askeri kargo ve nakliye uçağı.",
                PayloadCapacityTons = 37,
                CargoVolumeCubicMeters = 340,
                RangeWithMaxPayloadKm = 3300,
                CreatedAt = now,
                IsShowcase = true
            }
        );
    }
}
