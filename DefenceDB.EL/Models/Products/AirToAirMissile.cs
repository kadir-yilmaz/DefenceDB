namespace DefenceDB.EL.Models.Products;

public class AirToAirMissile : DefenseProduct
{
    public string? GuidanceType { get; set; }
    public double? MaxSpeedMach { get; set; }
    public string? FoxCode { get; set; }
    public double? RangeKm { get; set; }
}

