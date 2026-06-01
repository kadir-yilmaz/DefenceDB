using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models;

/// <summary>
/// Ürünler arası ilişki tablosu.
/// Örnek: Patriot (source) → uses_radar → AN/MPQ-65 (target)
/// RelationType: uses, variant_of, replaced_by, compatible_with, part_of
/// </summary>
public class ProductRelationship : BaseEntity
{
    public int SourceProductId { get; set; }
    public DefenseProduct SourceProduct { get; set; } = null!;

    public int TargetProductId { get; set; }
    public DefenseProduct TargetProduct { get; set; } = null!;

    [Required, MaxLength(50)]
    public string RelationType { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}
