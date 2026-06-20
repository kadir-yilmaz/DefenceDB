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
            var exists = await _context.Visitors
                .AnyAsync(v => v.VisitorHash == visitorHash);

            if (!exists)
            {
                var (os, browser) = ParseUserAgent(userAgent);
                var maskedIp = MaskIpAddress(ipAddress);

                // Yeni ziyaretçi
                var visitor = new Visitor
                {
                    VisitorHash = visitorHash,
                    FirstVisitDate = DateTime.UtcNow,
                    IpAddress = maskedIp,
                    Browser = browser,
                    OperatingSystem = os
                };

                _context.Visitors.Add(visitor);
                await _context.SaveChangesAsync();
                
                // Cache'i temizle
                await _cacheService.RemoveAsync(CACHE_KEY);
                
                _logger.LogInformation("New unique visitor tracked: {Hash} ({OS} - {Browser})", 
                    visitorHash.Substring(0, 8), os, browser);
            }
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

    /// <summary>
    /// User-Agent bilgisinden işletim sistemi (ve varsa mobil cihaz modeli) ile tarayıcıyı parse eder
    /// </summary>
    private (string Os, string Browser) ParseUserAgent(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return ("Bilinmeyen OS", "Bilinmeyen Tarayıcı");

        var ua = userAgent.ToLowerInvariant();

        // Tarayıcı tespiti
        string browser = "Bilinmeyen Tarayıcı";
        if (ua.Contains("edg/")) browser = "Edge";
        else if (ua.Contains("opr/") || ua.Contains("opera")) browser = "Opera";
        else if (ua.Contains("chrome") && !ua.Contains("chromium")) browser = "Chrome";
        else if (ua.Contains("firefox")) browser = "Firefox";
        else if (ua.Contains("safari") && !ua.Contains("chrome") && !ua.Contains("chromium")) browser = "Safari";

        // İşletim sistemi ve Cihaz tespiti
        string os = "Bilinmeyen OS";

        if (ua.Contains("windows"))
        {
            os = "Windows";
        }
        else if (ua.Contains("macintosh") || (ua.Contains("mac os x") && !ua.Contains("iphone") && !ua.Contains("ipad")))
        {
            os = "macOS";
        }
        else if (ua.Contains("iphone"))
        {
            os = "iPhone";
        }
        else if (ua.Contains("ipad"))
        {
            os = "iPad";
        }
        else if (ua.Contains("android"))
        {
            // Android cihaz modelini yakalamaya çalış
            os = "Android";
            try
            {
                int openParen = userAgent.IndexOf('(');
                int closeParen = userAgent.IndexOf(')');
                if (openParen != -1 && closeParen > openParen)
                {
                    var details = userAgent.Substring(openParen + 1, closeParen - openParen - 1);
                    var parts = details.Split(';');
                    
                    int androidIndex = -1;
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts[i].Contains("Android", StringComparison.OrdinalIgnoreCase))
                        {
                            androidIndex = i;
                            break;
                        }
                    }

                    if (androidIndex != -1 && androidIndex + 1 < parts.Length)
                    {
                        var modelPart = parts[androidIndex + 1].Trim();
                        
                        // Dil kodlarını atla (örn: tr-tr, en-us)
                        if (modelPart.Contains('-') && modelPart.Length <= 5)
                        {
                            if (androidIndex + 2 < parts.Length)
                            {
                                modelPart = parts[androidIndex + 2].Trim();
                            }
                        }

                        // Build bilgisini temizle
                        if (modelPart.Contains(" Build", StringComparison.OrdinalIgnoreCase))
                        {
                            int buildIdx = modelPart.IndexOf(" Build", StringComparison.OrdinalIgnoreCase);
                            modelPart = modelPart.Substring(0, buildIdx).Trim();
                        }

                        if (!string.IsNullOrWhiteSpace(modelPart))
                        {
                            os = $"Android ({modelPart})";
                        }
                    }
                }
            }
            catch
            {
                // Parse hatası durumunda fallback olarak "Android" kalır
            }
        }
        else if (ua.Contains("linux"))
        {
            os = "Linux";
        }

        return (os, browser);
    }

    /// <summary>
    /// IP adresini KVKK/GDPR uyumlu olacak şekilde maskeler (son blokları gizler)
    /// </summary>
    private string MaskIpAddress(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress) || ipAddress == "unknown")
            return "Bilinmeyen";

        if (ipAddress == "::1" || ipAddress == "127.0.0.1")
            return "Localhost";

        // IPv4 maskeleme: 192.168.1.15 -> 192.168.1.xxx
        if (ipAddress.Contains('.'))
        {
            var parts = ipAddress.Split('.');
            if (parts.Length == 4)
            {
                return $"{parts[0]}.{parts[1]}.{parts[2]}.xxx";
            }
        }
        // IPv6 maskeleme: 2001:db8:85a3:8d3:1319:8a2e:370:7334 -> 2001:db8:85a3:8d3:xxx:xxx:xxx:xxx
        else if (ipAddress.Contains(':'))
        {
            var parts = ipAddress.Split(':');
            if (parts.Length >= 4)
            {
                var maskedParts = parts.Take(4).ToList();
                while (maskedParts.Count < 8) maskedParts.Add("xxx");
                return string.Join(":", maskedParts);
            }
        }

        return ipAddress;
    }
}

/// <summary>
/// Cache için wrapper sınıfı (ICacheService class türü bekliyor)
/// </summary>
internal class CachedVisitorCount
{
    public int Count { get; set; }
}
