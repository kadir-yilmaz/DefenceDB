namespace DefenceDB.BLL.Abstract;

/// <summary>
/// Ziyaretçi takibi için servis interface'i
/// </summary>
public interface IVisitorService
{
    /// <summary>
    /// Ziyaretçiyi kaydet veya güncelle (async)
    /// </summary>
    Task TrackVisitorAsync(string ipAddress, string userAgent);
    
    /// <summary>
    /// Toplam benzersiz ziyaretçi sayısını döndür
    /// </summary>
    Task<int> GetTotalUniqueVisitorsAsync();
    
    /// <summary>
    /// 30 günden eski kayıtları sil (KVKK uyumu için)
    /// </summary>
    Task CleanupOldVisitorsAsync(int daysToKeep = 30);
}
