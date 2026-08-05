using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using DefenceDB.BLL.Abstract;
using DefenceDB.BLL.Concrete;
using DefenceDB.DAL.Seed;
using DefenceDB.EL.Extensions;
using NToastNotify;
using DefenceDB.WebUI.Services;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.DataProtection;
using DefenceDB.WebUI.Middleware;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Increase upload limits to ~100MB for multiple image uploads
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 104_857_600;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104_857_600;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104_857_600;
});

// ── Caching (In-Memory) ──────────────────────────────────────────
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

builder.Services.AddScoped<IImageProcessingService, ImageProcessingService>();
builder.Services.AddScoped<IProductFormMapper, ProductFormMapper>();

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("sqlConnection") 
    ?? throw new InvalidOperationException("Connection string 'sqlConnection' not found.");
builder.Services.AddSingleton<ReadModelCacheInterceptor>();
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString);
    options.AddInterceptors(serviceProvider.GetRequiredService<ReadModelCacheInterceptor>());
});

// Add Identity
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout settings (brute-force protection)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Account/Login";
    options.LogoutPath = "/Admin/Account/Logout";
    options.AccessDeniedPath = "/Admin/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.Name = "DefenceDB.Auth";
});

// Data Protection (Böylece deploy sonrası cookie/oturum anahtarları sıfırlanmaz ve kullanıcı çıkış yapmaz)
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys")))
    .SetApplicationName("DefenceDB");

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EditorPolicy", policy => policy.RequireRole("Admin", "Editor"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("Admin", "Editor", "User"));
});

// Register Services
builder.Services.AddScoped<ICategoryQueryService, CategoryQueryService>();
builder.Services.AddScoped<ICategoryCommandService, CategoryCommandService>();
builder.Services.AddScoped<IProductQueryService, ProductQueryService>();
builder.Services.AddScoped<IProductCommandService, ProductCommandService>();
builder.Services.AddScoped<IMascotSettingService, MascotSettingService>();
builder.Services.AddScoped<INotificationService, ToastNotificationService>();
builder.Services.AddScoped<IVisitorService, VisitorService>();

// Background Services
builder.Services.AddHostedService<DefenceDB.WebUI.BackgroundServices.VisitorCleanupService>();

builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions
    {
        ProgressBar = true,
        PositionClass = ToastPositions.BottomRight,
        TimeOut = 5000
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

// Seed roles and default admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        await SeedData.InitializeAsync(services, builder.Configuration);
        await EnsureArticleCategoriesSeededAsync(context, builder.Environment);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Statik dosyalar (css, js, images) için 7 gün tarayıcı cache
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=604800,immutable");
    }
});
app.UseNToastNotify();

// Visitor Tracking Middleware
app.UseMiddleware<VisitorTrackingMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Area route (must be before default route)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHealthChecks("/health");

// Temporary Debug Endpoint
app.MapGet("/debug/files", (IWebHostEnvironment env) =>
{
    var wwwroot = env.WebRootPath;
    if (string.IsNullOrEmpty(wwwroot) || !System.IO.Directory.Exists(wwwroot))
        return Results.Text("wwwroot not found or empty path.");

    var result = new System.Text.StringBuilder();
    result.AppendLine($"WebRootPath: {wwwroot}");
    
    void ListDir(string path, int indent)
    {
        var spaces = new string(' ', indent * 2);
        try
        {
            foreach (var dir in System.IO.Directory.GetDirectories(path))
            {
                result.AppendLine($"{spaces}[DIR] {System.IO.Path.GetFileName(dir)}");
                ListDir(dir, indent + 1);
            }
            foreach (var file in System.IO.Directory.GetFiles(path))
            {
                result.AppendLine($"{spaces}{System.IO.Path.GetFileName(file)} ({new System.IO.FileInfo(file).Length} bytes)");
            }
        }
        catch (Exception ex)
        {
            result.AppendLine($"{spaces}ERROR: {ex.Message}");
        }
    }

    ListDir(wwwroot, 0);
    return Results.Text(result.ToString());
});

app.MapGet("/debug/headers", (HttpContext context) =>
{
    var result = new System.Text.StringBuilder();
    result.AppendLine($"RemoteIpAddress: {context.Connection.RemoteIpAddress}");
    result.AppendLine($"LocalIpAddress: {context.Connection.LocalIpAddress}");
    result.AppendLine("\nHeaders:");
    foreach (var header in context.Request.Headers)
    {
        result.AppendLine($"{header.Key}: {header.Value}");
    }
    return Results.Text(result.ToString());
});


app.Run();

static async Task EnsureArticleCategoriesSeededAsync(AppDbContext context, IWebHostEnvironment environment)
{
    var jsonPath = Path.Combine(environment.WebRootPath, "data", "article-categories.json");
    if (!File.Exists(jsonPath))
        return;

    var json = await File.ReadAllTextAsync(jsonPath);
    var defaults = JsonSerializer.Deserialize<List<ArticleCategorySeedItem>>(
        json,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
    ) ?? new List<ArticleCategorySeedItem>();

    if (defaults.Count == 0)
        return;

    var existingCategories = await context.ArticleCategories
        .ToListAsync();

    var existingBySlug = existingCategories.ToDictionary(c => c.Slug, StringComparer.OrdinalIgnoreCase);
    var normalizedDefaults = defaults
        .Where(item => !string.IsNullOrWhiteSpace(item.Name))
        .Select(item => new
        {
            Name = item.Name.Trim(),
            Slug = string.IsNullOrWhiteSpace(item.Slug) ? item.Name.ToSlug() : item.Slug.ToSlug(),
            item.SortOrder
        })
        .ToList();

    foreach (var item in normalizedDefaults)
    {
        if (existingBySlug.TryGetValue(item.Slug, out var existing))
        {
            existing.Name = item.Name;
            existing.SortOrder = item.SortOrder;
            existing.UpdatedAt = DateTime.UtcNow;
        }
    }

    var categoriesToAdd = normalizedDefaults
        .Where(item => !existingBySlug.ContainsKey(item.Slug))
        .Select(item => new ArticleCategory
        {
            Name = item.Name,
            Slug = item.Slug,
            SortOrder = item.SortOrder
        })
        .ToList();

    if (categoriesToAdd.Count == 0)
    {
        await context.SaveChangesAsync();
        return;
    }

    await context.ArticleCategories.AddRangeAsync(categoriesToAdd);
    await context.SaveChangesAsync();
}

public sealed class ArticleCategorySeedItem
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public int SortOrder { get; set; }
}
