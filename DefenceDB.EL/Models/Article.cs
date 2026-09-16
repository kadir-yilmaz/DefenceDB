using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefenceDB.EL.Helpers;

namespace DefenceDB.EL.Models;

public class Article : BaseEntity
{
    [Required, MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    [NotMapped]
    public string Slug => ArticleSlugHelper.GenerateSlug(Title, Id);

    [MaxLength(500)]
    public string? Summary { get; set; }

    [Required]
    public string ContentMarkdown { get; set; } = string.Empty;

    public bool IsPublished { get; set; } = true;

    public bool IsShowcase { get; set; }

    public DateTime? PublishedAt { get; set; } = DateTime.UtcNow;

    public int ArticleCategoryId { get; set; }
    public ArticleCategory? ArticleCategory { get; set; }
}
