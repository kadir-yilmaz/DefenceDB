using System.Collections.Generic;

namespace DefenceDB.EL.Helpers;

public static class LocalizationHelper
{
    private static readonly Dictionary<string, string> TrNames = new()
    {
        // Temel Özellikler
        { "Name", "Ürün Adı" },
        { "Country", "Üretici Ülke" },
        { "Manufacturer", "Üretici Firma" },
        { "Status", "Durum" },
        { "IsActive", "Aktif mi?" },
        { "IsShowcase", "Vitrinde mi?" },
        { "VideoUrl", "Video Linki" },
        { "Description", "Açıklama" },

        // Genel Parametreler
        { "EngineType", "Motor Tipi" },
        { "MaxSeats", "Maksimum Koltuk Sayısı" },
        { "EngineHorsePower", "Motor Gücü (HP)" },
        { "WeightTons", "Ağırlık (Ton)" },
        { "CrewCount", "Mürettebat Sayısı" },
        { "PropulsionType", "İtki Sistemi" },

        // Deniz Araçları
        { "DisplacementTons", "Deplasman (Ton)" },
        { "MaxDepthMeters", "Maksimum Derinlik (Metre)" },
        { "TorpedoTubesCount", "Torpido Tüp Sayısı" },
        { "VlsCellsCount", "VLS Hücre Sayısı" },
        { "MaxSpeedKnots", "Maksimum Hız (Knot)" },
        { "SonarType", "Sonar Tipi" },

        // Kara Araçları
        { "MainGunCaliberMm", "Ana Silah Kalibresi (mm)" },
        { "HasAutoloader", "Otomatik Doldurucu" },
        { "ArmorThicknessMm", "Zırh Kalınlığı (mm)" },

        // Hava Araçları
        { "FighterGeneration", "Savaş Uçağı Nesli" },
        { "Generation", "Nesil" },
        { "HasStealth", "Hayalet Uçak Özelliği (Stealth)" },
        { "HasAesaRadar", "AESA Radar" },
        { "CombatRadiusKm", "Muharebe Yarıçapı (km)" },

        // Radar Sistemleri
        { "RadarType", "Radar Tipi" },
        { "ScanType", "Tarama Tipi" },
        { "MaxRangeKm", "Maksimum Menzil (km)" },
        { "TargetTrackingCapacity", "Hedef Takip Kapasitesi" },
        { "TrModuleCount", "T/R Modül Sayısı" },
        { "FrequencyBand", "Frekans Bandı" },
        { "ScanCoverage", "Tarama Kapsamı / Açısı" },
        { "CoolingSystem", "Soğutma Sistemi" },
        { "RadarSystemType", "Radar Sistem Tipi" },

        // Füzeler & Silahlar
        { "MaxSpeedMach", "Maksimum Hız (Mach)" },
        { "RangeKm", "Menzil (km)" },
        { "CepMeters", "Sapma Payı (CEP) (Metre)" },
        { "FlightAltitudeKm", "Uçuş İrtifası (km)" },
        { "WarheadWeightKg", "Savaş Başlığı Ağırlığı (kg)" },
        { "GuidanceType", "Güdüm Tipi" },
        { "FlightTimeMinutes", "Uçuş Süresi (Dakika)" },
        { "CarrierPlatform", "Taşıyıcı Platform" },
        { "CaliberMm", "Kalibre (mm)" },
        { "RateOfFireRpm", "Atış Hızı (Mermi/Dakika)" },
        { "FoxCode", "Fox Kodu" },
        { "EngagementRangeKm", "Angajman Menzili (km)" },
        { "MaxAltitudeKm", "Maksimum İrtifa (km)" },
        { "BallisticType", "Balistik Füze Tipi" },
        { "PayloadKg", "Taşıma Kapasitesi (Faydalı Yük) (kg)" },

        // Hava Savunma Sistemleri
        { "SystemType", "Sistem Tipi" },
        { "CokAlcakIrtifa", "Çok Alçak İrtifa" },
        { "AlcakIrtifa", "Alçak İrtifa" },
        { "OrtaIrtifa", "Orta İrtifa" },
        { "UzunYuksekIrtifa", "Uzun / Yüksek İrtifa" },
        { "MaxSearchRangeKm", "Maksimum Arama Menzili (km)" },
        { "MaxTrackingRangeKm", "Maksimum Takip Menzili (km)" },
        { "MaxEngagementAltitudeFt", "Maksimum Angajman İrtifası (feet)" },
        { "MaxTrackedTargets", "Eşzamanlı Takip Edilen Hedef Sayısı" },
        { "MissilesPerLauncher", "Fırlatıcı Başına Füze Sayısı" },
        { "HasAntiBallisticCapability", "Anti-Balistik Yeteneği" }
    };

    private static readonly Dictionary<string, string> EnNames = new()
    {
        // Core Properties
        { "Name", "Product Name" },
        { "Country", "Country of Origin" },
        { "Manufacturer", "Manufacturer" },
        { "Status", "Status" },
        { "IsActive", "Is Active" },
        { "IsShowcase", "Is Showcase" },
        { "VideoUrl", "Video URL" },
        { "Description", "Description" },

        // General Parameters
        { "EngineType", "Engine Type" },
        { "MaxSeats", "Max Seats" },
        { "EngineHorsePower", "Engine Horsepower (HP)" },
        { "WeightTons", "Weight (Tons)" },
        { "CrewCount", "Crew Count" },
        { "PropulsionType", "Propulsion Type" },

        // Naval
        { "DisplacementTons", "Displacement (Tons)" },
        { "MaxDepthMeters", "Max Depth (Meters)" },
        { "TorpedoTubesCount", "Torpedo Tubes Count" },
        { "VlsCellsCount", "VLS Cells Count" },
        { "MaxSpeedKnots", "Max Speed (Knots)" },
        { "SonarType", "Sonar Type" },

        // Land
        { "MainGunCaliberMm", "Main Gun Caliber (mm)" },
        { "HasAutoloader", "Has Autoloader" },
        { "ArmorThicknessMm", "Armor Thickness (mm)" },

        // Air
        { "FighterGeneration", "Fighter Generation" },
        { "Generation", "Generation" },
        { "HasStealth", "Has Stealth" },
        { "HasAesaRadar", "Has AESA Radar" },
        { "CombatRadiusKm", "Combat Radius (km)" },

        // Radar
        { "RadarType", "Radar Type" },
        { "ScanType", "Scan Type" },
        { "MaxRangeKm", "Max Range (km)" },
        { "TargetTrackingCapacity", "Target Tracking Capacity" },
        { "TrModuleCount", "T/R Module Count" },
        { "FrequencyBand", "Frequency Band" },
        { "ScanCoverage", "Scan Coverage" },
        { "CoolingSystem", "Cooling System" },
        { "RadarSystemType", "Radar System Type" },

        // Weapons
        { "MaxSpeedMach", "Max Speed (Mach)" },
        { "RangeKm", "Range (km)" },
        { "CepMeters", "CEP (Meters)" },
        { "FlightAltitudeKm", "Flight Altitude (km)" },
        { "WarheadWeightKg", "Warhead Weight (kg)" },
        { "GuidanceType", "Guidance Type" },
        { "FlightTimeMinutes", "Flight Time (Minutes)" },
        { "CarrierPlatform", "Carrier Platform" },
        { "CaliberMm", "Caliber (mm)" },
        { "RateOfFireRpm", "Rate of Fire (RPM)" },
        { "FoxCode", "Fox Code" },
        { "EngagementRangeKm", "Engagement Range (km)" },
        { "MaxAltitudeKm", "Max Altitude (km)" },
        { "BallisticType", "Ballistic Type" },
        { "PayloadKg", "Payload (kg)" },

        // Air Defense Systems
        { "SystemType", "System Type" },
        { "CokAlcakIrtifa", "Very Short Range" },
        { "AlcakIrtifa", "Short Range" },
        { "OrtaIrtifa", "Medium Range" },
        { "UzunYuksekIrtifa", "Long / High Altitude" },
        { "MaxSearchRangeKm", "Maximum Search Range (km)" },
        { "MaxTrackingRangeKm", "Maximum Tracking Range (km)" },
        { "MaxEngagementAltitudeFt", "Maximum Engagement Altitude (feet)" },
        { "MaxTrackedTargets", "Max Tracked Targets" },
        { "MissilesPerLauncher", "Missiles per Launcher" },
        { "HasAntiBallisticCapability", "Anti-Ballistic Capability" }
    };

    public static string GetDisplayName(string propertyName, string lang = "tr")
    {
        if (string.IsNullOrWhiteSpace(propertyName)) return propertyName;

        if (lang.ToLowerInvariant() == "en")
        {
            if (EnNames.TryGetValue(propertyName, out var enName))
                return enName;
        }
        else
        {
            if (TrNames.TryGetValue(propertyName, out var trName))
                return trName;
        }

        // Eğer sözlükte yoksa, mevcut sistem gibi kelimeleri ayırarak düzgün bir string döndür (Yedek Sistem)
        var result = System.Text.RegularExpressions.Regex.Replace(propertyName, "([a-z])([A-Z])", "$1 $2");
        if (result.StartsWith("Has ")) result = result.Substring(4);
        if (result.StartsWith("Is ")) result = result.Substring(3);
        
        return result;
    }
}
