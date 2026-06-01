namespace DefenceDB.EL.Models.Products;

public class NavalRadar : DefenseProduct
{
    public string? RadarType { get; set; }
    public string? ScanType { get; set; }
    public double? MaxRangeKm { get; set; }
}

