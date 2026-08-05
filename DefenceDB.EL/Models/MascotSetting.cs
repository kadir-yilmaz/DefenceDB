using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models;

/// <summary>
/// Defines a mascot message and links that appear on specific pages (e.g. category pages).
/// </summary>
public class MascotSetting : BaseEntity
{
    [Required, MaxLength(255)]
    [Display(Name = "Hedef Sayfa (Örn: /Category/balistik-fuzeler)")]
    public string TargetPath { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    [Display(Name = "Başlık")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Mesaj İçeriği")]
    public string Message { get; set; } = string.Empty;

    [Display(Name = "Makale/Link Listesi (JSON)")]
    public string? LinksJson { get; set; }

    [Display(Name = "Aktif Mi?")]
    public bool IsActive { get; set; } = true;
}
