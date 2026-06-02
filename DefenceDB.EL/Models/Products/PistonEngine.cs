using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class PistonEngine : DefenseProduct
{
    [Display(Name = "Beygir Gücü (HP)")]
    public double? HorsePower { get; set; }

    [Display(Name = "Tork (Nm)")]
    public double? TorqueNm { get; set; }

    [Display(Name = "Silindir Sayısı")]
    public int? Cylinders { get; set; }

    [Display(Name = "Yakıt Tipi")]
    [MaxLength(100)]
    public string? FuelType { get; set; }
}
