using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DefenceDB.EL.Models;
using DefenceDB.EL.Models.Products;
using DefenceDB.DAL.Seed;

namespace DefenceDB.DAL;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<DefenseProduct> DefenseProducts { get; set; } = null!;
    public DbSet<ProductRelationship> ProductRelationships { get; set; } = null!;
    public DbSet<ProductImage> ProductImages { get; set; } = null!;

    // Missiles
    public DbSet<BallisticMissile> BallisticMissiles { get; set; } = null!;
    public DbSet<AirToAirMissile> AirToAirMissiles { get; set; } = null!;
    public DbSet<AntiShipMissile> AntiShipMissiles { get; set; } = null!;
    public DbSet<AntiRadiationMissile> AntiRadiationMissiles { get; set; } = null!;
    public DbSet<CruiseMissile> CruiseMissiles { get; set; } = null!;
    public DbSet<HypersonicGlideVehicle> HypersonicGlideVehicles { get; set; } = null!;

    // Aircrafts
    public DbSet<BomberAircraft> BomberAircrafts { get; set; } = null!;
    public DbSet<FighterAircraft> FighterAircrafts { get; set; } = null!;
    public DbSet<TrainerAircraft> TrainerAircrafts { get; set; } = null!;

    // Ships
    public DbSet<FastAttackCraft> FastAttackCrafts { get; set; } = null!;
    public DbSet<Corvette> Corvettes { get; set; } = null!;
    public DbSet<Frigate> Frigates { get; set; } = null!;
    public DbSet<Destroyer> Destroyers { get; set; } = null!;
    public DbSet<Minehunter> Minehunters { get; set; } = null!;
    public DbSet<Submarine> Submarines { get; set; } = null!;

    // Radars
    public DbSet<AirborneRadar> AirborneRadars { get; set; } = null!;
    public DbSet<NavalRadar> NavalRadars { get; set; } = null!;
    public DbSet<AirDefenseRadar> AirDefenseRadars { get; set; } = null!;

    // Land Vehicles
    public DbSet<Tank> Tanks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // TPT Configuration
        modelBuilder.Entity<DefenseProduct>().ToTable("DefenseProducts");

        modelBuilder.Entity<BallisticMissile>().ToTable("BallisticMissiles");
        modelBuilder.Entity<AirToAirMissile>().ToTable("AirToAirMissiles");
        modelBuilder.Entity<AntiShipMissile>().ToTable("AntiShipMissiles");
        modelBuilder.Entity<AntiRadiationMissile>().ToTable("AntiRadiationMissiles");
        modelBuilder.Entity<CruiseMissile>().ToTable("CruiseMissiles");
        modelBuilder.Entity<HypersonicGlideVehicle>().ToTable("HypersonicGlideVehicles");

        modelBuilder.Entity<BomberAircraft>().ToTable("BomberAircrafts");
        modelBuilder.Entity<FighterAircraft>().ToTable("FighterAircrafts");
        modelBuilder.Entity<TrainerAircraft>().ToTable("TrainerAircrafts");

        modelBuilder.Entity<FastAttackCraft>().ToTable("FastAttackCrafts");
        modelBuilder.Entity<Corvette>().ToTable("Corvettes");
        modelBuilder.Entity<Frigate>().ToTable("Frigates");
        modelBuilder.Entity<Destroyer>().ToTable("Destroyers");
        modelBuilder.Entity<Minehunter>().ToTable("Minehunters");
        modelBuilder.Entity<Submarine>().ToTable("Submarines");

        modelBuilder.Entity<AirborneRadar>().ToTable("AirborneRadars");
        modelBuilder.Entity<NavalRadar>().ToTable("NavalRadars");
        modelBuilder.Entity<AirDefenseRadar>().ToTable("AirDefenseRadars");

        modelBuilder.Entity<Tank>().ToTable("Tanks");

        // Apply Product Seeds
        modelBuilder.SeedProducts();
    }
}
