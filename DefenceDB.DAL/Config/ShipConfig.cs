using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;

namespace DefenceDB.DAL.Config;

public class FrigateConfig : IEntityTypeConfiguration<Frigate>
{
    public void Configure(EntityTypeBuilder<Frigate> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new Frigate { Id = 35, CategoryId = 16, Name = "İstif Sınıfı (TCG İstanbul)", Slug = "İstif Sınıfı (TCG İstanbul)".ToSlug(), Manufacturer = "STM", Country = "Türkiye", DisplacementTons = 3000, VlsCellsCount = 16, CreatedAt = now },
            new Frigate { Id = 36, CategoryId = 16, Name = "FREMM Sınıfı", Slug = "FREMM Sınıfı".ToSlug(), Manufacturer = "Naval Group", Country = "Fransa", DisplacementTons = 6000, VlsCellsCount = 32, CreatedAt = now }
        );
    }
}

public class SubmarineConfig : IEntityTypeConfiguration<Submarine>
{
    public void Configure(EntityTypeBuilder<Submarine> builder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        builder.HasData(
            new Submarine { Id = 37, CategoryId = 18, Name = "Virginia Sınıfı", Slug = "Virginia Sınıfı".ToSlug(), Manufacturer = "General Dynamics", Country = "ABD", DisplacementTons = 7900, MaxDepthMeters = 240, TorpedoTubesCount = 4, PropulsionType = "Nükleer", CreatedAt = now },
            new Submarine { Id = 38, CategoryId = 18, Name = "Reis Sınıfı (Tip 214TN)", Slug = "Reis Sınıfı (Tip 214TN)".ToSlug(), Manufacturer = "Gölcük Tersanesi", Country = "Türkiye", DisplacementTons = 2010, MaxDepthMeters = 400, TorpedoTubesCount = 8, PropulsionType = "AIP (Hava Bağımsız)", CreatedAt = now }
        );
    }
}
