using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class InfantryWeapon : DefenseProduct
{
    [Display(Name = "Kalibre")]
    [MaxLength(100)]
    public string? Caliber { get; set; }

    [Display(Name = "Etkili Menzil (Metre)")]
    public int? EffectiveRangeMeters { get; set; }

    [Display(Name = "Atış Hızı (Mermi/Dakika)")]
    public int? RateOfFireRpm { get; set; }

    [Display(Name = "Ağırlık (kg)")]
    public double? WeightKg { get; set; }

    [Display(Name = "Şarjör Kapasitesi")]
    public int? MagazineCapacity { get; set; }
}
