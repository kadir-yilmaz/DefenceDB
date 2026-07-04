using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models;

public class ArticleCategory : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Slug { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
