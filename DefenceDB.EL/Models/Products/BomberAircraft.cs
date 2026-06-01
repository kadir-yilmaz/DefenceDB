namespace DefenceDB.EL.Models.Products;

public class BomberAircraft : DefenseProduct
{
    public double? PayloadCapacityKg { get; set; }
    public bool IsNuclearCapable { get; set; }
    public double? CombatRadiusKm { get; set; }
}

