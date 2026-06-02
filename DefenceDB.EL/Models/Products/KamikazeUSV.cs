using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class KamikazeUSV : DefenseProduct
{
    [Display(Name = "Harp Başlığı Ağırlığı (Kg)")]
    public double? WarheadWeightKg { get; set; }

    [Display(Name = "Menzil (nm)")]
    public double? RangeNauticalMiles { get; set; }

    [Display(Name = "Maksimum Hız (Knot)")]
    public double? MaxSpeedKnots { get; set; }
}
