namespace DefenceDB.EL.Models.Products;

public class AntiShipMissile : DefenseProduct
{
    public bool SeaSkimming { get; set; }
    public string? SpeedClass { get; set; }
    public double? RangeKm { get; set; }
    public double? MaxSpeedMach { get; set; }
}

