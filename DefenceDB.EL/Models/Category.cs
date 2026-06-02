using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models;

/// <summary>
/// Hiyerarşik kategori ağacı. Self-referencing yapı ile parent-child ilişkisi.
/// Örnek: Radar Sistemleri > Uçak Radarları > ...
/// </summary>
public class Category : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? IconClass { get; set; }

    [MaxLength(255)]
    public string? ModelTypeName { get; set; }

    public int SortOrder { get; set; }

    // Self-referencing hierarchy
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    // Products in this category
    public ICollection<DefenseProduct> Products { get; set; } = new List<DefenseProduct>();
}
