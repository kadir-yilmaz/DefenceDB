using DefenceDB.EL.Extensions;

namespace DefenceDB.EL.Helpers;

public static class ProductSlugHelper
{
    public const string Separator = "-p-";

    /// <summary>
    /// Generates a dynamic SEO slug: {nameSlug}-p-{base62Id}
    /// Example: "altay-ana-muharebe-tanki-p-1K"
    /// </summary>
    public static string GenerateSlug(string? name, int id)
    {
        var nameSlug = name?.ToSlug() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(nameSlug))
            nameSlug = "urun";

        var base62Id = Base62Converter.Encode(id);
        return $"{nameSlug}{Separator}{base62Id}";
    }

    /// <summary>
    /// Extracts the product ID from a dynamic slug.
    /// Handles:
    /// 1. {nameSlug}-p-{base62Id} (primary format)
    /// 2. p-{base62Id}
    /// 3. Legacy format: {id}-{oldSlug} or pure integer ID (backward compatibility)
    /// 4. Direct Base62 ID
    /// </summary>
    public static bool TryExtractId(string? slug, out int id)
    {
        id = 0;
        if (string.IsNullOrWhiteSpace(slug))
            return false;

        var clean = slug.Trim();

        // 1. Primary format: ends with -p-{base62Id}
        var lastSepIndex = clean.LastIndexOf(Separator, StringComparison.OrdinalIgnoreCase);
        if (lastSepIndex >= 0)
        {
            var base62Part = clean.Substring(lastSepIndex + Separator.Length);
            if (Base62Converter.TryDecode(base62Part, out id))
                return true;
        }

        // 2. Format: starts with p-{base62Id}
        if (clean.StartsWith("p-", StringComparison.OrdinalIgnoreCase))
        {
            var base62Part = clean.Substring(2);
            if (Base62Converter.TryDecode(base62Part, out id))
                return true;
        }

        // 3. Backward compatibility: Legacy format {id}-{oldSlug}
        var dashParts = clean.Split('-', 2);
        if (dashParts.Length >= 2 && int.TryParse(dashParts[0], out id) && id > 0)
            return true;

        // 4. Fallback: Pure integer ID
        if (int.TryParse(clean, out id) && id > 0)
            return true;

        // 5. Fallback: Entire string is Base62
        if (Base62Converter.TryDecode(clean, out id))
            return true;

        return false;
    }
}
