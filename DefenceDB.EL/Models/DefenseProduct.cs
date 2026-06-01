using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models;

/// <summary>
/// Tüm savunma ürünlerinin ana soyut sınıfı. TPT (Table-Per-Type) kalıtımı ile modeller bu sınıftan türer.
/// </summary>
public abstract class DefenseProduct : BaseEntity
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? NatoReportingName { get; set; }

    [MaxLength(5000)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [MaxLength(200)]
    public string? Manufacturer { get; set; }

    public int? YearIntroduced { get; set; }

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Active, Retired, In Development, Prototype
    /// </summary>
    [MaxLength(50)]
    public string? Status { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsShowcase { get; set; } = false;
    
    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    // Category FK
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    // Relationships (source = this product uses/relates to target)
    public ICollection<ProductRelationship> SourceRelationships { get; set; } = new List<ProductRelationship>();
    public ICollection<ProductRelationship> TargetRelationships { get; set; } = new List<ProductRelationship>();

    // Images
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
}
