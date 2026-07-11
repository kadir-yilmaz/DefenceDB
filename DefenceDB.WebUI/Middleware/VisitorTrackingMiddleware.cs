using DefenceDB.BLL.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace DefenceDB.WebUI.Middleware;

/// <summary>
/// Her HTTP isteğinde ziyaretçiyi otomatik olarak takip eden middleware.
/// Rıza gerektirmez — anonim istatistik çerezi kişisel veri toplamaz.
/// İlk ziyarette df_visitor_id cookie'si otomatik set edilir.
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
        var isGet = context.Request.Method == "GET";
        var path = context.Request.Path;
        var userAgent = context.Request.Headers["User-Agent"].ToString();

        // Statik dosya, API ve bot isteklerini atla
        if (!isGet || path.StartsWithSegments("/api") || IsStaticFile(path) || IsBot(userAgent))
        {
            await _next(context);
            return;
        }

        string? visitorId = null;

        // df_visitor_id cookie kontrolü
        if (context.Request.Cookies.TryGetValue("df_visitor_id", out var existingId) &&
            !string.IsNullOrWhiteSpace(existingId) &&
            existingId != "rejected") // Eski "rejected" cookie'leri varsa yeni UUID ile değiştir
        {
            visitorId = existingId;
        }
        else
        {
            // Yeni ziyaretçi — otomatik UUID oluştur ve cookie set et
            visitorId = Guid.NewGuid().ToString("N");

            context.Response.Cookies.Append("df_visitor_id", visitorId, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }

        // Response'u gönder
        await _next(context);

        // Response gönderildikten sonra visitor tracking yap (fire-and-forget)
        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var visitorService = scope.ServiceProvider.GetRequiredService<IVisitorService>();

                await visitorService.TrackVisitorAsync(visitorId, userAgent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in background visitor tracking");
            }
        });
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
