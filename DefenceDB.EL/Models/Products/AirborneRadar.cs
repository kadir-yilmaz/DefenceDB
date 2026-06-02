namespace DefenceDB.EL.Models.Products;

public class AirborneRadar : DefenseProduct
{
    public string? RadarType { get; set; }
    public double? MaxRangeKm { get; set; }
    public int? TargetTrackingCapacity { get; set; }
    
    // Yeni eklenen radar özellikleri
    public int? TrModuleCount { get; set; }
    public string? FrequencyBand { get; set; }
    public string? ScanCoverage { get; set; }
    public string? CoolingSystem { get; set; }
}

