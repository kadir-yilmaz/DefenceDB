using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class GasTurbineEngine : DefenseProduct
{
    [Display(Name = "Maksimum İtki (lbf)")]
    public double? MaxThrustLbf { get; set; }

    [Display(Name = "Kuru İtki (lbf)")]
    public double? DryThrustLbf { get; set; }

    [Display(Name = "Art Yakıcı (Afterburner) Var Mı?")]
    public bool HasAfterburner { get; set; }

    [Display(Name = "Bypass Oranı")]
    public double? BypassRatio { get; set; }

    [Display(Name = "Şaft Beygir Gücü (SHP)")]
    public double? ShaftHorsePowerHp { get; set; }
}
