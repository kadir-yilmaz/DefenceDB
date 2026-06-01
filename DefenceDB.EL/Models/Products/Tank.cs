using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class Tank : DefenseProduct
{
    [Display(Name = "Motor Gücü (Beygir)")]
    public int? EngineHorsePower { get; set; }

    [Display(Name = "Top Çapı (mm)")]
    public double? MainGunCaliberMm { get; set; }

    [Display(Name = "Ağırlık (Ton)")]
    public double? WeightTons { get; set; }

    [Display(Name = "Mürettebat Sayısı")]
    public int? CrewCount { get; set; }

    [Display(Name = "Otomatik Doldurucu")]
    public bool HasAutoloader { get; set; }
}
