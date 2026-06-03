namespace DefenceDB.EL.Models;

/// <summary>
/// Elasticsearch indekslemesi ve benchmark karşılaştırması için kullanılan
/// düzleştirilmiş (denormalized) ürün dokümanı.
/// TPT JOIN'lerinden kaçınmak için tüm alanlar tek bir flat yapıda tutulur.
/// </summary>
public class ProductDocument
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? NatoReportingName { get; set; }
    public string? Description { get; set; }
    public string? Country { get; set; }
    public string? Manufacturer { get; set; }
    public int? YearIntroduced { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Status { get; set; }
    public bool IsActive { get; set; }
    public bool IsShowcase { get; set; }
    public string? VideoUrl { get; set; }

    // Category info (denormalized)
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;

    // Product type discriminator (e.g. "FighterAircraft", "Submarine")
    public string ProductType { get; set; } = string.Empty;

    // TPT alt tablo alanları — JSON string olarak saklanır (Elasticsearch'de nested obje)
    public Dictionary<string, object?> SpecificProperties { get; set; } = new();

    // Main image URL (denormalized)
    public string? MainImageUrl { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
