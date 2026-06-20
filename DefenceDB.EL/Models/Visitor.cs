namespace DefenceDB.EL.Models;

/// <summary>
/// Benzersiz ziyaretçi takibi için model.
/// KVKK uyumlu: IP adresi saklanmaz, günlük salt ile hash kullanılır.
/// </summary>
public class Visitor
{
    public int Id { get; set; }
    
    /// <summary>
    /// IP + UserAgent + Tarih kombinasyonunun hash'i.
    /// Günlük salt kullanılarak anonimleştirilir.
    /// </summary>
    public string VisitorHash { get; set; } = null!;
    
    /// <summary>
    /// İlk ziyaret tarihi (UTC)
    /// </summary>
    public DateTime FirstVisitDate { get; set; } = DateTime.UtcNow;

    public string? IpAddress { get; set; }
    public string? Browser { get; set; }
    public string? OperatingSystem { get; set; }
}
