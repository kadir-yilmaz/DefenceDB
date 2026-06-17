using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class AirToAirMissile : DefenseProduct
{
    public string? GuidanceType { get; set; }
    public double? MaxSpeedMach { get; set; }

    [Display(Name = "Fox Kodu")]
    public byte? FoxCode { get; set; }

    public double? RangeKm { get; set; }
}

