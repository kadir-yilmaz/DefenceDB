namespace DefenceDB.EL.Models.Products;

public class FighterAircraft : DefenseProduct
{
    public string? Generation { get; set; }
    public bool HasStealth { get; set; }
    public bool HasAesaRadar { get; set; }
    public double? CombatRadiusKm { get; set; }
}

