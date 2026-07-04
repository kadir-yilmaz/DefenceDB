using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DefenceDB.DAL.Config;

public class ArticleConfig : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Title).IsRequired().HasMaxLength(180);
        builder.Property(a => a.Slug).IsRequired().HasMaxLength(220);
        builder.Property(a => a.Summary).HasMaxLength(500);
        builder.Property(a => a.ContentMarkdown).IsRequired();
        builder.Property(a => a.IsPublished).HasDefaultValue(true);
        builder.Property(a => a.IsShowcase).HasDefaultValue(false);

        builder.HasIndex(a => a.Slug).IsUnique();
        builder.HasIndex(a => a.PublishedAt);
        builder.HasIndex(a => a.IsShowcase);

        builder.HasOne(a => a.ArticleCategory)
            .WithMany(c => c.Articles)
            .HasForeignKey(a => a.ArticleCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
