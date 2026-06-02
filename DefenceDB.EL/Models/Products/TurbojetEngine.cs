using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class TurbojetEngine : DefenseProduct
{
    [Display(Name = "Maksimum İtki (lbf)")]
    public double? MaxThrustLbf { get; set; }

    [Display(Name = "Kuru İtki (lbf)")]
    public double? DryThrustLbf { get; set; }

    [Display(Name = "Art Yakıcı (Afterburner) Var Mı?")]
    public bool HasAfterburner { get; set; }
}
