using System.Security.Cryptography;
using System.Text;
using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DefenceDB.BLL.Concrete;

/// <summary>
/// KVKK uyumlu ziyaretçi takip servisi.
/// IP adresi saklanmaz, günlük salt ile hash kullanılır.
/// </summary>
public class VisitorService : IVisitorService
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ILogger<VisitorService> _logger;
    private const string CACHE_KEY = "TotalUniqueVisitors";
    private const int CACHE_DURATION_MINUTES = 5;

    public VisitorService(
        AppDbContext context,
        ICacheService cacheService,
        ILogger<VisitorService> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task TrackVisitorAsync(string visitorId, string ipAddress, string userAgent)
    {
        try
        {
            // Bot kontrolü - basit filtre
            if (IsBot(userAgent))
            {
                _logger.LogDebug("Bot detected, skipping: {UserAgent}", userAgent);
                return;
            }

            var visitorHash = GenerateVisitorHash(visitorId);

            // Bu hash ile ziyaretçi var mı?
            var existingVisitor = await _context.Visitors
                .FirstOrDefaultAsync(v => v.VisitorHash == visitorHash);

            if (existingVisitor == null)
            {
                // Yeni ziyaretçi
                var visitor = new Visitor
                {
                    VisitorHash = visitorHash,
                    FirstVisitDate = DateTime.UtcNow,
                    LastVisitDate = DateTime.UtcNow,
                    VisitCount = 1
                };

                _context.Visitors.Add(visitor);
                await _context.SaveChangesAsync();
                
                // Cache'i temizle
                await _cacheService.RemoveAsync(CACHE_KEY);
                
                _logger.LogInformation("New unique visitor tracked: {Hash}", visitorHash.Substring(0, 8));
            }
            else if ((DateTime.UtcNow - existingVisitor.LastVisitDate).TotalMinutes >= 30)
            {
                // 30 dakikadan fazla geçmiş (yeni oturum), güncelle
                existingVisitor.LastVisitDate = DateTime.UtcNow;
                existingVisitor.VisitCount++;
                await _context.SaveChangesAsync();
                
                _logger.LogDebug("Visitor session updated: {Hash}", visitorHash.Substring(0, 8));
            }
            // 30 dakika içinde ziyaret varsa hiçbir şey yapma (veritabanı yükünü azaltmak için)
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking visitor");
            // Visitor tracking hatası uygulamayı etkilememeli
        }
    }

    public async Task<int> GetTotalUniqueVisitorsAsync()
    {
        // Cache'den dene
        var cachedCount = await _cacheService.GetAsync<CachedVisitorCount>(CACHE_KEY);
        if (cachedCount != null)
        {
            return cachedCount.Count;
        }

        // Cache yoksa veritabanından al
        var count = await _context.Visitors.CountAsync();
        
        // Cache'e kaydet
        await _cacheService.SetAsync(CACHE_KEY, new CachedVisitorCount { Count = count }, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
        
        return count;
    }

    public async Task CleanupOldVisitorsAsync(int daysToKeep = 30)
    {
        try
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);
            
            var oldVisitors = await _context.Visitors
                .Where(v => v.FirstVisitDate < cutoffDate)
                .ToListAsync();

            if (oldVisitors.Any())
            {
                _context.Visitors.RemoveRange(oldVisitors);
                await _context.SaveChangesAsync();
                
                // Cache'i temizle
                await _cacheService.RemoveAsync(CACHE_KEY);
                
                _logger.LogInformation("Cleaned up {Count} old visitor records", oldVisitors.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up old visitors");
        }
    }

    /// <summary>
    /// KVKK uyumlu: visitorId (GUID) hash'i. IP adresi veya kişisel veri içermez.
    /// Sabit salt ile tek yönlü SHA256 kullanılarak anonimleştirilir.
    /// </summary>
    private string GenerateVisitorHash(string visitorId)
    {
        var salt = "DefenceDB-Visitor-Static-Salt-2026";
        var combined = $"{visitorId}|{salt}";
        
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(combined);
        var hash = sha256.ComputeHash(bytes);
        
        return Convert.ToHexString(hash).ToLower();
    }

    /// <summary>
    /// Basit bot algılama
    /// </summary>
    private bool IsBot(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return true;

        var botKeywords = new[]
        {
            "bot", "crawler", "spider", "scraper", "http", "curl", "wget",
            "python", "java", "go-http", "axios", "fetch", "postman"
        };

        var lowerUserAgent = userAgent.ToLower();
        return botKeywords.Any(keyword => lowerUserAgent.Contains(keyword));
    }
}

/// <summary>
/// Cache için wrapper sınıfı (ICacheService class türü bekliyor)
/// </summary>
internal class CachedVisitorCount
{
    public int Count { get; set; }
}
