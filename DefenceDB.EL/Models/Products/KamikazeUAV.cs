using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class KamikazeUAV : DefenseProduct
{
    [Display(Name = "Harp Başlığı Ağırlığı (Kg)")]
    public double? WarheadWeightKg { get; set; }

    [Display(Name = "Havada Kalış Süresi (Saat)")]
    public double? EnduranceHours { get; set; }

    [Display(Name = "Menzil (Km)")]
    public double? RangeKm { get; set; }

    [Display(Name = "Maksimum Hız (km/s)")]
    public double? MaxSpeedKmh { get; set; }
}
