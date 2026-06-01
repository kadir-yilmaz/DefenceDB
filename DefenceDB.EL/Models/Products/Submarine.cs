namespace DefenceDB.EL.Models.Products;

public class Submarine : DefenseProduct
{
    public double? DisplacementTons { get; set; }
    public double? MaxDepthMeters { get; set; }
    public int? TorpedoTubesCount { get; set; }
    public string? PropulsionType { get; set; }
}

