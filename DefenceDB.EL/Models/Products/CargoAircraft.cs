namespace DefenceDB.EL.Models.Products;

public class CargoAircraft : DefenseProduct
{
    public double? PayloadCapacityTons { get; set; }
    public double? CargoVolumeCubicMeters { get; set; }
    public double? RangeWithMaxPayloadKm { get; set; }
}
