using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class USV : DefenseProduct
{
    [Display(Name = "Görev Süresi (Saat)")]
    public double? EnduranceHours { get; set; }

    [Display(Name = "Maksimum Hız (Knot)")]
    public double? MaxSpeedKnots { get; set; }

    [Display(Name = "Deplasman (Ton)")]
    public double? DisplacementTons { get; set; }

    [Display(Name = "Operasyonel Menzil (nm)")]
    public double? OperationalRangeNauticalMiles { get; set; }
}
