using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class ElectricNuclearPower : DefenseProduct
{
    [Display(Name = "Güç Çıkışı (MW)")]
    public double? PowerOutputMw { get; set; }

    [Display(Name = "Sistem Tipi (Nükleer/AIP vb.)")]
    [MaxLength(150)]
    public string? SystemType { get; set; }
}
