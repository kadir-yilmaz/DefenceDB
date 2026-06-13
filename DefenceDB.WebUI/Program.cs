using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using DefenceDB.BLL.Abstract;
using DefenceDB.BLL.Concrete;
using DefenceDB.DAL.Seed;
using NToastNotify;
using DefenceDB.WebUI.Services;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.DataProtection;
using DefenceDB.WebUI.Middleware;

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

// ── Feature Toggle Manager ──────────────────────────────────────
builder.Services.AddSingleton<IFeatureManager, FeatureManager>();
var featureManager = new FeatureManager(builder.Configuration);

// ── Caching (In-Memory) ──────────────────────────────────────────
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

builder.Services.AddScoped<IImageProcessingService, ImageProcessingService>();
builder.Services.AddScoped<IProductFormMapper, ProductFormMapper>();

// ── Search (Elasticsearch veya SQL Fallback) ────────────────────
if (featureManager.UseElasticsearch)
{
    var esUrl = builder.Configuration.GetConnectionString("Elasticsearch") ?? "http://localhost:9200";
    var settings = new ElasticsearchClientSettings(new Uri(esUrl))
        .DefaultIndex("defencedb-products")
        .DisableDirectStreaming()
        .RequestTimeout(TimeSpan.FromSeconds(30));
    
    builder.Services.AddSingleton(new ElasticsearchClient(settings));
    builder.Services.AddScoped<ISearchService, ElasticsearchService>();
}
else
{
    builder.Services.AddScoped<ISearchService, SqlFallbackSearchService>();
}

// ── EF Core Sync Interceptor ────────────────────────────────────
builder.Services.AddSingleton<ElasticSyncInterceptor>();

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("sqlConnection") 
    ?? throw new InvalidOperationException("Connection string 'sqlConnection' not found.");
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString);
    
    // Interceptor'ı sadece feature aktifse ekle
    var fm = serviceProvider.GetService<IFeatureManager>();
    if (fm is not null)
    {
        var interceptor = serviceProvider.GetRequiredService<ElasticSyncInterceptor>();
        options.AddInterceptors(interceptor);
    }
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
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductQueryService, ProductQueryManager>();
builder.Services.AddScoped<IProductCommandService, ProductCommandManager>();
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

        // Elasticsearch ilk indeksleme (feature aktifse)
        var fm = services.GetService<IFeatureManager>();
        if (fm is not null && fm.UseElasticsearch)
        {
            var searchService = services.GetRequiredService<ISearchService>();
            await searchService.ReindexAllAsync();
        }
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
app.UseStaticFiles();
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

app.Run();
