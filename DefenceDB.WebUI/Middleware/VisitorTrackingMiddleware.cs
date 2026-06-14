using DefenceDB.BLL.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace DefenceDB.WebUI.Middleware;

/// <summary>
/// Her HTTP isteğinde ziyaretçiyi takip eden middleware
/// </summary>
public class VisitorTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<VisitorTrackingMiddleware> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public VisitorTrackingMiddleware(
        RequestDelegate next,
        ILogger<VisitorTrackingMiddleware> logger,
        IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Request verilerini asenkron thread'e geçmeden önce kopyalıyoruz (ObjectDisposedException engellemek için)
        var isGet = context.Request.Method == "GET";
        var path = context.Request.Path;
        var ipAddress = GetClientIpAddress(context);
        var userAgent = context.Request.Headers["User-Agent"].ToString();

        // Önce response'u gönder
        await _next(context);

        // Response gönderildikten sonra visitor tracking yap (fire-and-forget)
        _ = Task.Run(async () =>
        {
            try
            {
                // Sadece GET isteklerini ve HTML sayfalarını say
                if (isGet && 
                    !path.StartsWithSegments("/api") &&
                    !IsStaticFile(path))
                {
                    // Yeni scope oluştur (DbContext thread-safe olması için)
                    using var scope = _scopeFactory.CreateScope();
                    var visitorService = scope.ServiceProvider.GetRequiredService<IVisitorService>();
                    
                    await visitorService.TrackVisitorAsync(ipAddress, userAgent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in background visitor tracking");
                // Hata durumunda da uygulama devam etsin
            }
        });
    }

    /// <summary>
    /// Gerçek client IP adresini al (proxy/CDN desteği ile)
    /// </summary>
    private string GetClientIpAddress(HttpContext context)
    {
        // X-Forwarded-For header'ı kontrol et (proxy/CDN arkasındaysak)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            // İlk IP'yi al (gerçek client IP'si)
            return forwardedFor.Split(',')[0].Trim();
        }

        // X-Real-IP header'ı kontrol et (nginx vb.)
        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        // Fallback: RemoteIpAddress
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    /// <summary>
    /// Statik dosya kontrolü (css, js, images vb. sayılmasın)
    /// </summary>
    private bool IsStaticFile(PathString path)
    {
        var staticExtensions = new[] { ".css", ".js", ".jpg", ".jpeg", ".png", ".gif", 
            ".svg", ".ico", ".woff", ".woff2", ".ttf", ".eot", ".map", ".json" };
        
        return staticExtensions.Any(ext => path.Value?.EndsWith(ext, StringComparison.OrdinalIgnoreCase) ?? false);
    }
}
