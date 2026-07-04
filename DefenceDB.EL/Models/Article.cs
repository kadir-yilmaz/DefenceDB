using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models;

public class Article : BaseEntity
{
    [Required, MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

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
