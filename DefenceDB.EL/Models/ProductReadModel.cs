using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DefenceDB.EL.Models;

/// <summary>
/// CQRS (Read Model) mimarisi için denormalize edilmiş, düzleştirilmiş (flat) ürün tablosu.
/// TPT (Table-Per-Type) JOIN'lerinden kaçınmak için tüm temel verileri ve spesifik özellikleri
/// tek bir tabloda tutar.
/// </summary>
public class ProductReadModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // Id'si DefenseProduct ile aynı olacak
    public int Id { get; set; }
    
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string Slug { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? NatoReportingName { get; set; }
    
    public string? Description { get; set; }
    
    [MaxLength(100)]
    public string? Country { get; set; }
    
    [MaxLength(200)]
    public string? Manufacturer { get; set; }
    
    public int? YearIntroduced { get; set; }
    
    public string? ThumbnailUrl { get; set; }
    
    [MaxLength(50)]
    public string? Status { get; set; }
    
    public bool IsActive { get; set; }
    public bool IsShowcase { get; set; }
    
    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    // Kategori Bilgileri (Denormalize)
    public int CategoryId { get; set; }
    
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string CategorySlug { get; set; } = string.Empty;

    // Ürün Tipi (Örn: FighterAircraft, Submarine)
    [MaxLength(100)]
    public string ProductType { get; set; } = string.Empty;

    // Ana Görsel (Denormalize)
    public string? MainImageUrl { get; set; }

    // Alt sınıfların spesifik özellikleri JSON formatında
    public string SpecificPropertiesJson { get; set; } = "{}";

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
