namespace DefenceDB.EL.Models.Products;

public class AwacsAircraft : DefenseProduct
{
    public string? RadarType { get; set; }
    public double? DetectionRangeKm { get; set; }
    public int? MaxTrackedTargets { get; set; }
}
