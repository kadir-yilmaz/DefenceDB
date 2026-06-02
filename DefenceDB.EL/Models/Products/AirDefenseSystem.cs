using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class AirDefenseSystem : DefenseProduct
{
    [Display(Name = "Maksimum Arama Menzili (km)")]
    public double? MaxSearchRangeKm { get; set; }

    [Display(Name = "Maksimum Takip Menzili (km)")]
    public double? MaxTrackingRangeKm { get; set; }

    [Display(Name = "Maksimum Angajman İrtifası (feet)")]
    public double? MaxEngagementAltitudeFt { get; set; }

    [Display(Name = "Eşzamanlı Takip Edilen Hedef Sayısı")]
    public int? MaxTrackedTargets { get; set; }

    [Display(Name = "Fırlatıcı Başına Füze Sayısı")]
    public int? MissilesPerLauncher { get; set; }

    [Display(Name = "Anti-Balistik Yeteneği")]
    public bool HasAntiBallisticCapability { get; set; }
}
