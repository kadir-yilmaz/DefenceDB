using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class RocketMotor : DefenseProduct
{
    [Display(Name = "İtki Gücü (kN)")]
    public double? ThrustKn { get; set; }

    [Display(Name = "Yanma Süresi (Saniye)")]
    public double? BurnTimeSeconds { get; set; }

    [Display(Name = "Yakıt Tipi (Katı/Sıvı)")]
    [MaxLength(100)]
    public string? PropellantType { get; set; }
}
