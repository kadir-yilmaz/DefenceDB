using DefenceDB.EL.Extensions;

namespace DefenceDB.EL.Helpers;

public static class ArticleSlugHelper
{
    public const string Separator = "-a-";

    /// <summary>
    /// Generates a dynamic SEO slug: {titleSlug}-a-{base62Id}
    /// Example: "bayraktar-tb3-test-ucuslari-a-1A"
    /// </summary>
    public static string GenerateSlug(string? title, int id)
    {
        var titleSlug = title?.ToSlug() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(titleSlug))
            titleSlug = "makale";

        var base62Id = Base62Converter.Encode(id);
        return $"{titleSlug}{Separator}{base62Id}";
    }

    /// <summary>
    /// Extracts the article ID from a dynamic slug.
    /// Handles:
    /// 1. {titleSlug}-a-{base62Id} (primary format)
    /// 2. a-{base62Id}
    /// 3. {base62Id}
    /// 4. Plain numeric ID
    /// </summary>
    public static bool TryExtractId(string? slug, out int id)
    {
        id = 0;
        if (string.IsNullOrWhiteSpace(slug))
            return false;

        var clean = slug.Trim();

        // 1. Primary format: ends with -a-{base62Id}
        var lastSepIndex = clean.LastIndexOf(Separator, StringComparison.OrdinalIgnoreCase);
        if (lastSepIndex >= 0)
        {
            var base62Part = clean.Substring(lastSepIndex + Separator.Length);
            if (Base62Converter.TryDecode(base62Part, out id))
                return true;
        }

        // 2. Format: starts with a-{base62Id}
        if (clean.StartsWith("a-", StringComparison.OrdinalIgnoreCase))
        {
            var base62Part = clean.Substring(2);
            if (Base62Converter.TryDecode(base62Part, out id))
                return true;
        }

        // 3. Fallback: Entire string is Base62
        if (Base62Converter.TryDecode(clean, out id))
            return true;

        // 4. Fallback: Entire string is integer ID
        if (int.TryParse(clean, out id) && id > 0)
            return true;

        return false;
    }
}
