using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DefenceDB.EL.Models;
using DefenceDB.EL.Models.Products;

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
    public DbSet<AirSojAircraft> AirSojAircrafts { get; set; } = null!;
    public DbSet<CargoAircraft> CargoAircrafts { get; set; } = null!;
    public DbSet<MaritimePatrolAircraft> MaritimePatrolAircrafts { get; set; } = null!;
    public DbSet<AwacsAircraft> AwacsAircrafts { get; set; } = null!;

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

    // Air Defense Systems
    public DbSet<AirDefenseSystem> AirDefenseSystems { get; set; } = null!;

    // Unmanned Platforms
    public DbSet<UAV> UAVs { get; set; } = null!;
    public DbSet<USV> USVs { get; set; } = null!;
    public DbSet<UGV> UGVs { get; set; } = null!;
    public DbSet<KamikazeUAV> KamikazeUAVs { get; set; } = null!;
    public DbSet<KamikazeUSV> KamikazeUSVs { get; set; } = null!;

    // Engines and Propulsion Systems
    public DbSet<PistonEngine> PistonEngines { get; set; }
    public DbSet<RocketMotor> RocketMotors { get; set; }
    public DbSet<ElectricNuclearPower> ElectricNuclearPowers { get; set; }
    
    public DbSet<TurbofanEngine> TurbofanEngines { get; set; }
    public DbSet<TurbojetEngine> TurbojetEngines { get; set; }
    public DbSet<TurbopropEngine> TurbopropEngines { get; set; }
    public DbSet<TurboshaftEngine> TurboshaftEngines { get; set; }
    public DbSet<MarineGasTurbine> MarineGasTurbines { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply Configurations from Config folder
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
        modelBuilder.Entity<AirSojAircraft>().ToTable("AirSojAircrafts");
        modelBuilder.Entity<CargoAircraft>().ToTable("CargoAircrafts");
        modelBuilder.Entity<MaritimePatrolAircraft>().ToTable("MaritimePatrolAircrafts");
        modelBuilder.Entity<AwacsAircraft>().ToTable("AwacsAircrafts");

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
        modelBuilder.Entity<AirDefenseSystem>().ToTable("AirDefenseSystems");

        modelBuilder.Entity<UAV>().ToTable("UAVs");
        modelBuilder.Entity<USV>().ToTable("USVs");
        modelBuilder.Entity<UGV>().ToTable("UGVs");
        modelBuilder.Entity<KamikazeUAV>().ToTable("KamikazeUAVs");
        modelBuilder.Entity<KamikazeUSV>().ToTable("KamikazeUSVs");

        modelBuilder.Entity<PistonEngine>().ToTable("PistonEngines");
        modelBuilder.Entity<RocketMotor>().ToTable("RocketMotors");
        modelBuilder.Entity<ElectricNuclearPower>().ToTable("ElectricNuclearPowers");
        
        modelBuilder.Entity<TurbofanEngine>().ToTable("TurbofanEngines");
        modelBuilder.Entity<TurbojetEngine>().ToTable("TurbojetEngines");
        modelBuilder.Entity<TurbopropEngine>().ToTable("TurbopropEngines");
        modelBuilder.Entity<TurboshaftEngine>().ToTable("TurboshaftEngines");
        modelBuilder.Entity<MarineGasTurbine>().ToTable("MarineGasTurbines");
    }
}
