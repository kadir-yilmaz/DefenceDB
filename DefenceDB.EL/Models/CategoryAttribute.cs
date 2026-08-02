using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models;

/// <summary>
/// Kategoriye özel dinamik attribute tanımı.
/// Admin panelden her kategoriye istenen sayıda attribute eklenebilir.
/// Ürünlerin Specs dictionary'sindeki key'ler bu attribute'lardaki Name değerleridir.
/// </summary>
public class CategoryAttribute : BaseEntity
{
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Attribute'un teknik adı (Specs dictionary key'i olarak kullanılır).
    /// Örn: "MaxSpeedKmh", "HasStealth", "Generation"
    /// </summary>
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcıya görünen ad.
    /// Örn: "Maksimum Hız (km/s)", "Stealth Özelliği", "Nesil"
    /// </summary>
    [Required, MaxLength(200)]
    public string DisplayName { get; set; } = string.Empty;

    public AttributeType Type { get; set; }

    /// <summary>
    /// Dropdown tipi için seçenek listesi. JSON olarak saklanır.
    /// </summary>
    public List<string>? Options { get; set; }

    public bool IsRequired { get; set; }

    public int DisplayOrder { get; set; }
}
