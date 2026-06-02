using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;

namespace DefenceDB.DAL.Config;

public class TurbofanEngineConfig : IEntityTypeConfiguration<TurbofanEngine>
{
    public void Configure(EntityTypeBuilder<TurbofanEngine> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new TurbofanEngine { Id = 301, CategoryId = 31, Name = "TEI-TF6000", Slug = "TEI-TF6000".ToSlug(), Manufacturer = "TEI", Country = "Türkiye", MaxThrustLbf = 6000, DryThrustLbf = 6000, HasAfterburner = false, BypassRatio = 1.08, CreatedAt = now, IsShowcase = true },
            new TurbofanEngine { Id = 302, CategoryId = 31, Name = "TEI-TF10000", Slug = "TEI-TF10000".ToSlug(), Manufacturer = "TEI", Country = "Türkiye", MaxThrustLbf = 10000, DryThrustLbf = 6000, HasAfterburner = true, BypassRatio = 1.08, CreatedAt = now },
            new TurbofanEngine { Id = 303, CategoryId = 31, Name = "F135-PW-100", Slug = "F135-PW-100".ToSlug(), Manufacturer = "Pratt & Whitney", Country = "ABD", MaxThrustLbf = 43000, DryThrustLbf = 28000, HasAfterburner = true, BypassRatio = 0.57, CreatedAt = now },
            new TurbofanEngine { Id = 304, CategoryId = 31, Name = "F110-GE-129", Slug = "F110-GE-129".ToSlug(), Manufacturer = "General Electric", Country = "ABD", MaxThrustLbf = 29500, DryThrustLbf = 17155, HasAfterburner = true, BypassRatio = 0.76, CreatedAt = now },
            new TurbofanEngine { Id = 305, CategoryId = 31, Name = "AL-31F", Slug = "AL-31F".ToSlug(), Manufacturer = "NPO Saturn", Country = "Rusya", MaxThrustLbf = 27560, DryThrustLbf = 17130, HasAfterburner = true, BypassRatio = 0.59, CreatedAt = now }
        );
    }
}

public class TurbojetEngineConfig : IEntityTypeConfiguration<TurbojetEngine>
{
    public void Configure(EntityTypeBuilder<TurbojetEngine> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new TurbojetEngine { Id = 306, CategoryId = 35, Name = "Kale KTJ-3200", Slug = "kale-ktj-3200", Manufacturer = "Kale Arge", Country = "Türkiye", MaxThrustLbf = 720, DryThrustLbf = 720, HasAfterburner = false, CreatedAt = now, IsShowcase = true }
        );
    }
}

public class TurboshaftEngineConfig : IEntityTypeConfiguration<TurboshaftEngine>
{
    public void Configure(EntityTypeBuilder<TurboshaftEngine> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new TurboshaftEngine { Id = 307, CategoryId = 38, Name = "TEI-TS1400", Slug = "tei-ts1400", Manufacturer = "TEI", Country = "Türkiye", ShaftHorsePowerHp = 1400, CreatedAt = now, IsShowcase = true }
        );
    }
}

public class TurbopropEngineConfig : IEntityTypeConfiguration<TurbopropEngine>
{
    public void Configure(EntityTypeBuilder<TurbopropEngine> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new TurbopropEngine { Id = 308, CategoryId = 36, Name = "PT6A-67A", Slug = "pt6a-67a", Manufacturer = "Pratt & Whitney Canada", Country = "Kanada", ShaftHorsePowerHp = 1200, CreatedAt = now }
        );
    }
}

public class MarineGasTurbineConfig : IEntityTypeConfiguration<MarineGasTurbine>
{
    public void Configure(EntityTypeBuilder<MarineGasTurbine> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new MarineGasTurbine { Id = 309, CategoryId = 37, Name = "GE LM2500", Slug = "ge-lm2500", Manufacturer = "General Electric", Country = "ABD", ShaftHorsePowerHp = 33600, CreatedAt = now }
        );
    }
}

public class PistonEngineConfig : IEntityTypeConfiguration<PistonEngine>
{
    public void Configure(EntityTypeBuilder<PistonEngine> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new PistonEngine { Id = 311, CategoryId = 32, Name = "TEI-PD170", Slug = "TEI-PD170".ToSlug(), Manufacturer = "TEI", Country = "Türkiye", HorsePower = 170, Cylinders = 4, FuelType = "Dizel / JP-8", CreatedAt = now, IsShowcase = true },
            new PistonEngine { Id = 312, CategoryId = 32, Name = "BATU", Slug = "BATU".ToSlug(), Manufacturer = "BMC Power", Country = "Türkiye", HorsePower = 1500, Cylinders = 12, FuelType = "Dizel", CreatedAt = now, IsShowcase = true },
            new PistonEngine { Id = 313, CategoryId = 32, Name = "MTU MB 873 Ka-501", Slug = "MTU-MB-873".ToSlug(), Manufacturer = "MTU", Country = "Almanya", HorsePower = 1500, Cylinders = 12, FuelType = "Dizel", CreatedAt = now }
        );
    }
}

public class RocketMotorConfig : IEntityTypeConfiguration<RocketMotor>
{
    public void Configure(EntityTypeBuilder<RocketMotor> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new RocketMotor { Id = 321, CategoryId = 33, Name = "Roketsan Katı Yakıtlı Roket Motoru", Slug = "Roketsan-Kati-Yakitli-Motor".ToSlug(), Manufacturer = "Roketsan", Country = "Türkiye", PropellantType = "Katı", CreatedAt = now },
            new RocketMotor { Id = 322, CategoryId = 33, Name = "Raptor", Slug = "Raptor-Engine".ToSlug(), Manufacturer = "SpaceX", Country = "ABD", ThrustKn = 2200, PropellantType = "Sıvı (Metan/LOX)", CreatedAt = now }
        );
    }
}

public class ElectricNuclearPowerConfig : IEntityTypeConfiguration<ElectricNuclearPower>
{
    public void Configure(EntityTypeBuilder<ElectricNuclearPower> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new ElectricNuclearPower { Id = 331, CategoryId = 34, Name = "S9G Nükleer Reaktör", Slug = "S9G-Nuclear-Reactor".ToSlug(), Manufacturer = "General Electric", Country = "ABD", PowerOutputMw = 30, SystemType = "Nükleer Reaktör", CreatedAt = now },
            new ElectricNuclearPower { Id = 332, CategoryId = 34, Name = "Siemens PEM Yakıt Hücresi (AIP)", Slug = "Siemens-PEM-AIP".ToSlug(), Manufacturer = "Siemens", Country = "Almanya", PowerOutputMw = 0.24, SystemType = "AIP (Hava Bağımsız Tahrik)", CreatedAt = now }
        );
    }
}
