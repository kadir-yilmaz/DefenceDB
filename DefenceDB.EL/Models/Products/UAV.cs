using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class UAV : DefenseProduct
{
    [Display(Name = "Havada Kalış Süresi (Saat)")]
    public double? EnduranceHours { get; set; }

    [Display(Name = "Maksimum İrtifa (Feet)")]
    public int? MaxAltitudeFeet { get; set; }

    [Display(Name = "Faydalı Yük Kapasitesi (Kg)")]
    public double? PayloadCapacityKg { get; set; }

    [Display(Name = "Kanat Açıklığı (Metre)")]
    public double? WingSpanMeters { get; set; }

    [Display(Name = "Seyir Hızı (km/s)")]
    public double? CruisingSpeedKmh { get; set; }
}
