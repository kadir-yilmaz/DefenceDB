namespace DefenceDB.EL.Models;

/// <summary>
/// Tüm entity'ler için ortak base sınıf.
/// Id, oluşturulma ve güncellenme tarihi içerir.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
