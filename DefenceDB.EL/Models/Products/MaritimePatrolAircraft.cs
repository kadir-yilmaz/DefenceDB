namespace DefenceDB.EL.Models.Products;

public class MaritimePatrolAircraft : DefenseProduct
{
    public double? EnduranceHours { get; set; }
    public string? SonarType { get; set; }
    public bool HasTorpedoTubes { get; set; }
}
