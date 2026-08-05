using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DefenceDB.EL.Models;

namespace DefenceDB.DAL;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<CategoryAttribute> CategoryAttributes { get; set; } = null!;
    public DbSet<ArticleCategory> ArticleCategories { get; set; } = null!;
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<DefenseProduct> DefenseProducts { get; set; } = null!;
    public DbSet<ProductRelationship> ProductRelationships { get; set; } = null!;
    public DbSet<ProductImage> ProductImages { get; set; } = null!;
    public DbSet<MascotSetting> MascotSettings { get; set; } = null!;

    // Visitor Tracking
    public DbSet<Visitor> Visitors { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply Configurations from Config folder
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Visitor Configuration
        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.VisitorHash).IsRequired().HasMaxLength(64);
            entity.Property(v => v.Browser).HasMaxLength(50);
            entity.Property(v => v.OperatingSystem).HasMaxLength(50);
            entity.HasIndex(v => v.VisitorHash).IsUnique();
            entity.HasIndex(v => v.FirstVisitDate);
        });
    }
}
