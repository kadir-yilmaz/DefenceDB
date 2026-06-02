using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class UGV : DefenseProduct
{
    [Display(Name = "Ağırlık (Kg)")]
    public double? WeightKg { get; set; }

    [Display(Name = "Maksimum Hız (km/s)")]
    public double? MaxSpeedKmh { get; set; }

    [Display(Name = "Operasyonel Menzil (Km)")]
    public double? OperationalRangeKm { get; set; }

    [Display(Name = "Yürüyüş Tipi")]
    [MaxLength(100)]
    public string? DriveType { get; set; }
}
