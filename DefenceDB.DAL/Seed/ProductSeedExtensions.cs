using Microsoft.EntityFrameworkCore;
using DefenceDB.EL.Models.Products;
using DefenceDB.EL.Extensions;

namespace DefenceDB.DAL.Seed;

public static class ProductSeedExtensions
{
    public static void SeedProducts(this ModelBuilder modelBuilder)
    {
        var now = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // ----------------------------------------------------
        // 1. FIGHTER AIRCRAFTS (Bol Veri) - CategoryId: 11
        // ----------------------------------------------------
        modelBuilder.Entity<FighterAircraft>().HasData(
            new FighterAircraft { Id = 1, CategoryId = 11, Name = "F-16 Fighting Falcon", Slug = "F-16 Fighting Falcon".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", Description = "Çok amaçlı 4. nesil savaş uçağı.", Generation = "4", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 550, CreatedAt = now, IsShowcase = true },
            new FighterAircraft { Id = 2, CategoryId = 11, Name = "F-35 Lightning II", Slug = "F-35 Lightning II".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", Description = "5. nesil çok amaçlı hayalet savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 1090, CreatedAt = now, IsShowcase = true },
            new FighterAircraft { Id = 3, CategoryId = 11, Name = "F-22 Raptor", Slug = "F-22 Raptor".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", Description = "Hava üstünlüğü sağlayan 5. nesil savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 850, CreatedAt = now },
            new FighterAircraft { Id = 4, CategoryId = 11, Name = "KAAN", Slug = "KAAN".ToSlug(), Manufacturer = "TUSAŞ", Country = "Türkiye", Description = "Milli Muharip Uçak (MMU). 5. nesil çok rollü savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 1100, CreatedAt = now, IsShowcase = true },
            new FighterAircraft { Id = 5, CategoryId = 11, Name = "Eurofighter Typhoon", Slug = "Eurofighter Typhoon".ToSlug(), Manufacturer = "Eurofighter Jagdflugzeug", Country = "Avrupa Birliği", Description = "Çift motorlu, delta kanatlı çok rollü savaş uçağı.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 1390, CreatedAt = now },
            new FighterAircraft { Id = 6, CategoryId = 11, Name = "Dassault Rafale", Slug = "Dassault Rafale".ToSlug(), Manufacturer = "Dassault Aviation", Country = "Fransa", Description = "Omnirole savaş uçağı.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 1850, CreatedAt = now },
            new FighterAircraft { Id = 7, CategoryId = 11, Name = "Su-57", Slug = "Su-57".ToSlug(), Manufacturer = "Sukhoi", Country = "Rusya", Description = "Rusya'nın 5. nesil savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 1500, CreatedAt = now },
            new FighterAircraft { Id = 8, CategoryId = 11, Name = "Su-35", Slug = "Su-35".ToSlug(), Manufacturer = "Sukhoi", Country = "Rusya", Description = "Gelişmiş 4++ nesil çok amaçlı savaş uçağı.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 1600, CreatedAt = now },
            new FighterAircraft { Id = 9, CategoryId = 11, Name = "J-20 Mighty Dragon", Slug = "J-20 Mighty Dragon".ToSlug(), Manufacturer = "Chengdu", Country = "Çin", Description = "Çin'in 5. nesil ağır savaş uçağı.", Generation = "5", HasStealth = true, HasAesaRadar = true, CombatRadiusKm = 2000, CreatedAt = now },
            new FighterAircraft { Id = 10, CategoryId = 11, Name = "JAS 39 Gripen", Slug = "JAS 39 Gripen".ToSlug(), Manufacturer = "Saab", Country = "İsveç", Description = "Hafif, tek motorlu çok amaçlı uçak.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 800, CreatedAt = now },
            new FighterAircraft { Id = 11, CategoryId = 11, Name = "F-15EX Eagle II", Slug = "F-15EX Eagle II".ToSlug(), Manufacturer = "Boeing", Country = "ABD", Description = "F-15'in modernize edilmiş en gelişmiş versiyonu.", Generation = "4.5", HasStealth = false, HasAesaRadar = true, CombatRadiusKm = 1270, CreatedAt = now }
        );

        // ----------------------------------------------------
        // 2. AIR-TO-AIR MISSILES (Bol Veri) - CategoryId: 5
        // ----------------------------------------------------
        modelBuilder.Entity<AirToAirMissile>().HasData(
            new AirToAirMissile { Id = 12, CategoryId = 5, Name = "AIM-9 Sidewinder", Slug = "AIM-9 Sidewinder".ToSlug(), Manufacturer = "Raytheon", Country = "ABD", GuidanceType = "Infrared (IR)", MaxSpeedMach = 2.5, FoxCode = "Fox 2", CreatedAt = now },
            new AirToAirMissile { Id = 13, CategoryId = 5, Name = "AIM-120 AMRAAM", Slug = "AIM-120 AMRAAM".ToSlug(), Manufacturer = "Raytheon", Country = "ABD", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.0, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 14, CategoryId = 5, Name = "Meteor", Slug = "Meteor".ToSlug(), Manufacturer = "MBDA", Country = "Avrupa Birliği", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.0, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 15, CategoryId = 5, Name = "IRIS-T", Slug = "IRIS-T".ToSlug(), Manufacturer = "Diehl Defence", Country = "Almanya", GuidanceType = "Infrared (IR)", MaxSpeedMach = 3.0, FoxCode = "Fox 2", CreatedAt = now },
            new AirToAirMissile { Id = 16, CategoryId = 5, Name = "GÖKDOĞAN", Slug = "GÖKDOĞAN".ToSlug(), Manufacturer = "TÜBİTAK SAGE", Country = "Türkiye", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.0, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 17, CategoryId = 5, Name = "BOZDOĞAN", Slug = "BOZDOĞAN".ToSlug(), Manufacturer = "TÜBİTAK SAGE", Country = "Türkiye", GuidanceType = "Infrared (IR)", MaxSpeedMach = 4.0, FoxCode = "Fox 2", CreatedAt = now },
            new AirToAirMissile { Id = 18, CategoryId = 5, Name = "R-77 (AA-12 Adder)", Slug = "R-77 (AA-12 Adder)".ToSlug(), Manufacturer = "Vympel", Country = "Rusya", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.0, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 19, CategoryId = 5, Name = "R-73 (AA-11 Archer)", Slug = "R-73 (AA-11 Archer)".ToSlug(), Manufacturer = "Vympel", Country = "Rusya", GuidanceType = "Infrared (IR)", MaxSpeedMach = 2.5, FoxCode = "Fox 2", CreatedAt = now },
            new AirToAirMissile { Id = 20, CategoryId = 5, Name = "PL-15", Slug = "PL-15".ToSlug(), Manufacturer = "CASC", Country = "Çin", GuidanceType = "Aktif Radar", MaxSpeedMach = 4.5, FoxCode = "Fox 3", CreatedAt = now },
            new AirToAirMissile { Id = 21, CategoryId = 5, Name = "MICA", Slug = "MICA".ToSlug(), Manufacturer = "MBDA", Country = "Fransa", GuidanceType = "IR/RF", MaxSpeedMach = 3.0, FoxCode = "Fox 2 / Fox 3", CreatedAt = now }
        );

        // ----------------------------------------------------
        // 3. RADARS (Bol Veri) - Airborne (20), Naval (21), AirDefense (19)
        // ----------------------------------------------------
        modelBuilder.Entity<AirborneRadar>().HasData(
            new AirborneRadar { Id = 22, CategoryId = 20, Name = "AN/APG-81", Slug = "AN/APG-81".ToSlug(), Manufacturer = "Northrop Grumman", Country = "ABD", RadarType = "AESA", MaxRangeKm = 160, TargetTrackingCapacity = 23, CreatedAt = now },
            new AirborneRadar { Id = 23, CategoryId = 20, Name = "AN/APG-77", Slug = "AN/APG-77".ToSlug(), Manufacturer = "Northrop Grumman", Country = "ABD", RadarType = "AESA", MaxRangeKm = 240, TargetTrackingCapacity = 100, CreatedAt = now },
            new AirborneRadar { Id = 24, CategoryId = 20, Name = "Captor-E", Slug = "Captor-E".ToSlug(), Manufacturer = "Euroradar", Country = "Avrupa Birliği", RadarType = "AESA", MaxRangeKm = 200, TargetTrackingCapacity = 60, CreatedAt = now },
            new AirborneRadar { Id = 25, CategoryId = 20, Name = "Irbis-E", Slug = "Irbis-E".ToSlug(), Manufacturer = "NIIP", Country = "Rusya", RadarType = "PESA", MaxRangeKm = 400, TargetTrackingCapacity = 30, CreatedAt = now },
            new AirborneRadar { Id = 26, CategoryId = 20, Name = "MURAD", Slug = "MURAD".ToSlug(), Manufacturer = "Aselsan", Country = "Türkiye", RadarType = "AESA", MaxRangeKm = 150, TargetTrackingCapacity = 40, CreatedAt = now }
        );

        modelBuilder.Entity<AirDefenseRadar>().HasData(
            new AirDefenseRadar { Id = 27, CategoryId = 19, Name = "AN/SPY-1", Slug = "AN/SPY-1".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", RadarType = "PESA", MaxRangeKm = 310, TargetTrackingCapacity = 100, CreatedAt = now },
            new AirDefenseRadar { Id = 28, CategoryId = 19, Name = "Ers-Int (Erken İhbar)", Slug = "Ers-Int (Erken İhbar)".ToSlug(), Manufacturer = "Aselsan", Country = "Türkiye", RadarType = "AESA", MaxRangeKm = 600, TargetTrackingCapacity = 200, CreatedAt = now },
            new AirDefenseRadar { Id = 29, CategoryId = 19, Name = "Patriot AN/MPQ-65", Slug = "Patriot AN/MPQ-65".ToSlug(), Manufacturer = "Raytheon", Country = "ABD", RadarType = "AESA", MaxRangeKm = 150, TargetTrackingCapacity = 100, CreatedAt = now },
            new AirDefenseRadar { Id = 30, CategoryId = 19, Name = "S-400 91N6E", Slug = "S-400 91N6E".ToSlug(), Manufacturer = "Almaz-Antey", Country = "Rusya", RadarType = "AESA", MaxRangeKm = 600, TargetTrackingCapacity = 300, CreatedAt = now }
        );

        // ----------------------------------------------------
        // 4. DİĞERLERİ (Gerisinde Birkaç Tane)
        // ----------------------------------------------------
        
        // Anti-Ship Missiles (Id: 7)
        modelBuilder.Entity<AntiShipMissile>().HasData(
            new AntiShipMissile { Id = 31, CategoryId = 7, Name = "Harpoon", Slug = "Harpoon".ToSlug(), Manufacturer = "Boeing", Country = "ABD", SeaSkimming = true, SpeedClass = "Subsonic", RangeKm = 140, CreatedAt = now },
            new AntiShipMissile { Id = 32, CategoryId = 7, Name = "ATMACA", Slug = "ATMACA".ToSlug(), Manufacturer = "Roketsan", Country = "Türkiye", SeaSkimming = true, SpeedClass = "Subsonic", RangeKm = 220, CreatedAt = now, IsShowcase = true }
        );

        // Ballistic Missiles (Id: 6)
        modelBuilder.Entity<BallisticMissile>().HasData(
            new BallisticMissile { Id = 33, CategoryId = 6, Name = "Trident II D5", Slug = "Trident II D5".ToSlug(), Manufacturer = "Lockheed Martin", Country = "ABD", RangeKm = 12000, PayloadKg = 2800, IsNuclearCapable = true, HasMirv = true, CreatedAt = now },
            new BallisticMissile { Id = 34, CategoryId = 6, Name = "TAYFUN", Slug = "TAYFUN".ToSlug(), Manufacturer = "Roketsan", Country = "Türkiye", RangeKm = 560, PayloadKg = 500, IsNuclearCapable = false, HasMirv = false, CreatedAt = now }
        );

        // Frigates (Id: 16)
        modelBuilder.Entity<Frigate>().HasData(
            new Frigate { Id = 35, CategoryId = 16, Name = "İstif Sınıfı (TCG İstanbul)", Slug = "İstif Sınıfı (TCG İstanbul)".ToSlug(), Manufacturer = "STM", Country = "Türkiye", DisplacementTons = 3000, VlsCellsCount = 16, CreatedAt = now },
            new Frigate { Id = 36, CategoryId = 16, Name = "FREMM Sınıfı", Slug = "FREMM Sınıfı".ToSlug(), Manufacturer = "Naval Group", Country = "Fransa", DisplacementTons = 6000, VlsCellsCount = 32, CreatedAt = now }
        );

        // Submarines (Id: 18)
        modelBuilder.Entity<Submarine>().HasData(
            new Submarine { Id = 37, CategoryId = 18, Name = "Virginia Sınıfı", Slug = "Virginia Sınıfı".ToSlug(), Manufacturer = "General Dynamics", Country = "ABD", DisplacementTons = 7900, MaxDepthMeters = 240, TorpedoTubesCount = 4, PropulsionType = "Nükleer", CreatedAt = now },
            new Submarine { Id = 38, CategoryId = 18, Name = "Reis Sınıfı (Tip 214TN)", Slug = "Reis Sınıfı (Tip 214TN)".ToSlug(), Manufacturer = "Gölcük Tersanesi", Country = "Türkiye", DisplacementTons = 2010, MaxDepthMeters = 400, TorpedoTubesCount = 8, PropulsionType = "AIP (Hava Bağımsız)", CreatedAt = now }
        );

        // Cruise Missiles (Id: 8)
        modelBuilder.Entity<CruiseMissile>().HasData(
            new CruiseMissile { Id = 39, CategoryId = 8, Name = "Tomahawk", Slug = "Tomahawk".ToSlug(), Manufacturer = "Raytheon", Country = "ABD", RangeKm = 1600, CepMeters = 10, CreatedAt = now },
            new CruiseMissile { Id = 40, CategoryId = 8, Name = "SOM", Slug = "SOM".ToSlug(), Manufacturer = "TÜBİTAK SAGE / Roketsan", Country = "Türkiye", RangeKm = 250, CepMeters = 5, CreatedAt = now }
        );

        // ----------------------------------------------------
        // 7. TANKS (Bol Veri) - CategoryId: 22
        // ----------------------------------------------------
        modelBuilder.Entity<Tank>().HasData(
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

        // ----------------------------------------------------
        // 8. UNMANNED AERIAL VEHICLES (İHA) - CategoryId: 24
        // ----------------------------------------------------
        modelBuilder.Entity<UAV>().HasData(
            new UAV { Id = 201, CategoryId = 24, Name = "Bayraktar TB2", Slug = "Bayraktar TB2".ToSlug(), Manufacturer = "Baykar", Country = "Türkiye", EnduranceHours = 27, MaxAltitudeFeet = 25000, PayloadCapacityKg = 150, WingSpanMeters = 12, CruisingSpeedKmh = 130, CreatedAt = now, IsShowcase = true },
            new UAV { Id = 202, CategoryId = 24, Name = "Bayraktar TB3", Slug = "Bayraktar TB3".ToSlug(), Manufacturer = "Baykar", Country = "Türkiye", EnduranceHours = 24, MaxAltitudeFeet = 25000, PayloadCapacityKg = 280, WingSpanMeters = 14, CruisingSpeedKmh = 160, CreatedAt = now, IsShowcase = true },
            new UAV { Id = 203, CategoryId = 24, Name = "Bayraktar Akıncı", Slug = "Bayraktar Akıncı".ToSlug(), Manufacturer = "Baykar", Country = "Türkiye", EnduranceHours = 24, MaxAltitudeFeet = 40000, PayloadCapacityKg = 1500, WingSpanMeters = 20, CruisingSpeedKmh = 277, CreatedAt = now, IsShowcase = true },
            new UAV { Id = 204, CategoryId = 24, Name = "Bayraktar Kızılelma", Slug = "Bayraktar Kızılelma".ToSlug(), Manufacturer = "Baykar", Country = "Türkiye", EnduranceHours = 5, MaxAltitudeFeet = 45000, PayloadCapacityKg = 1500, WingSpanMeters = 10, CruisingSpeedKmh = 735, CreatedAt = now, IsShowcase = true },
            
            new UAV { Id = 205, CategoryId = 24, Name = "MQ-9 Reaper", Slug = "MQ-9 Reaper".ToSlug(), Manufacturer = "General Atomics", Country = "ABD", EnduranceHours = 27, MaxAltitudeFeet = 50000, PayloadCapacityKg = 1700, WingSpanMeters = 20, CruisingSpeedKmh = 313, CreatedAt = now },
            new UAV { Id = 206, CategoryId = 24, Name = "RQ-4 Global Hawk", Slug = "RQ-4 Global Hawk".ToSlug(), Manufacturer = "Northrop Grumman", Country = "ABD", EnduranceHours = 34, MaxAltitudeFeet = 60000, PayloadCapacityKg = 1360, WingSpanMeters = 39.9, CruisingSpeedKmh = 575, CreatedAt = now },
            
            new UAV { Id = 207, CategoryId = 24, Name = "Wing Loong II", Slug = "Wing Loong II".ToSlug(), Manufacturer = "Chengdu", Country = "Çin", EnduranceHours = 32, MaxAltitudeFeet = 32500, PayloadCapacityKg = 480, WingSpanMeters = 20.5, CruisingSpeedKmh = 370, CreatedAt = now },
            new UAV { Id = 208, CategoryId = 24, Name = "CH-4 Rainbow", Slug = "CH-4 Rainbow".ToSlug(), Manufacturer = "CASC", Country = "Çin", EnduranceHours = 40, MaxAltitudeFeet = 23600, PayloadCapacityKg = 345, WingSpanMeters = 18, CruisingSpeedKmh = 235, CreatedAt = now },
            
            new UAV { Id = 209, CategoryId = 24, Name = "Orion", Slug = "Orion".ToSlug(), Manufacturer = "Kronshtadt Group", Country = "Rusya", EnduranceHours = 24, MaxAltitudeFeet = 24600, PayloadCapacityKg = 250, WingSpanMeters = 16.3, CruisingSpeedKmh = 120, CreatedAt = now },
            new UAV { Id = 210, CategoryId = 24, Name = "S-70 Okhotnik", Slug = "S-70 Okhotnik".ToSlug(), Manufacturer = "Sukhoi", Country = "Rusya", EnduranceHours = 12, MaxAltitudeFeet = 34400, PayloadCapacityKg = 2800, WingSpanMeters = 20, CruisingSpeedKmh = 1000, CreatedAt = now },
            
            new UAV { Id = 211, CategoryId = 24, Name = "Anka-S", Slug = "Anka-S".ToSlug(), Manufacturer = "TUSAŞ", Country = "Türkiye", EnduranceHours = 30, MaxAltitudeFeet = 30000, PayloadCapacityKg = 350, WingSpanMeters = 17.5, CruisingSpeedKmh = 200, CreatedAt = now },
            new UAV { Id = 212, CategoryId = 24, Name = "Aksungur", Slug = "Aksungur".ToSlug(), Manufacturer = "TUSAŞ", Country = "Türkiye", EnduranceHours = 50, MaxAltitudeFeet = 40000, PayloadCapacityKg = 750, WingSpanMeters = 24, CruisingSpeedKmh = 250, CreatedAt = now, IsShowcase = true },
            new UAV { Id = 213, CategoryId = 24, Name = "Anka-3", Slug = "Anka-3".ToSlug(), Manufacturer = "TUSAŞ", Country = "Türkiye", EnduranceHours = 10, MaxAltitudeFeet = 40000, PayloadCapacityKg = 1200, WingSpanMeters = 12, CruisingSpeedKmh = 800, CreatedAt = now, IsShowcase = true }
        );
    }
}
