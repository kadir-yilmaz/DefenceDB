namespace DefenceDB.EL.Models.Products;

public class BallisticMissile : DefenseProduct
{
    public double? RangeKm { get; set; }
    public double? PayloadKg { get; set; }
    public bool IsNuclearCapable { get; set; }
    public bool HasMirv { get; set; }
    public double? MaxSpeedMach { get; set; }
    public string? BallisticType { get; set; }
}

