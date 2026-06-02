using System.Text.RegularExpressions;
using Markdig;

namespace DefenceDB.EL.Extensions;

public static class StringExtensions
{
    public static string ToSlug(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Türkçe karakterleri çevir
        string str = text.Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("ö", "o").Replace("ç", "c")
                         .Replace("İ", "i").Replace("Ğ", "g").Replace("Ü", "u").Replace("Ş", "s").Replace("Ö", "o").Replace("Ç", "c");

        str = str.ToLowerInvariant();
        
        // Sadece harf, rakam ve boşluk/tire kalsın
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        
        // Çoklu boşlukları tek boşluğa çevir ve trimle
        str = Regex.Replace(str, @"\s+", " ").Trim();
        
        // En fazla 50 karakter al
        str = str.Substring(0, str.Length <= 50 ? str.Length : 50).Trim();
        
        // Boşlukları tireye çevir
        str = Regex.Replace(str, @"\s", "-");
        
        return str;
    }

    public static string? GetYouTubeVideoId(this string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return null;

        var regex = new Regex(@"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^""&?\/\s]{11})", RegexOptions.IgnoreCase);
        var match = regex.Match(url);
        
        return match.Success ? match.Groups[1].Value : null;
    }

    public static string ToHtmlFormat(this string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;

        // Markdig Pipeline oluştur ve Advanced Extensions (Tablo desteği vb.) ekle
        var pipeline = new Markdig.MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        return Markdig.Markdown.ToHtml(text, pipeline);
    }
}
