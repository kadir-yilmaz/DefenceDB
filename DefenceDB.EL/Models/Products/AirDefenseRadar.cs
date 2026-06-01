namespace DefenceDB.EL.Models.Products;

public class AirDefenseRadar : DefenseProduct
{
    public string? RadarType { get; set; }
    public double? MaxRangeKm { get; set; }
    public int? TargetTrackingCapacity { get; set; }
}

