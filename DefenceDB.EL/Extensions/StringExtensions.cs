using System.Text.RegularExpressions;

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

    public static string ToUserFriendlyName(this string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;
        
        // Split camel case: HasAesaRadar -> Has Aesa Radar
        var result = Regex.Replace(text, "([a-z])([A-Z])", "$1 $2");
        
        // Temizlemeler
        if (result.StartsWith("Has ")) result = result.Substring(4);
        if (result.StartsWith("Is ")) result = result.Substring(3);

        result = result.Replace(" Km", " (km)")
                       .Replace(" Kg", " (kg)")
                       .Replace(" Mach", " (Mach)");
                       
        return result;
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

        // 1. Standartlaştırma
        string normalized = text.Replace("\r\n", "\n");

        // 2. Basit Markdown desteği (**kalın** -> <strong>kalın</strong>)
        normalized = Regex.Replace(normalized, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
        normalized = Regex.Replace(normalized, @"\*(.+?)\*", "<em>$1</em>");

        // 3. Kırık Paragraf Düzeltmesi: 
        // Eğer bir paragraf boşluğu (\n\n) nokta, ünlem, soru işareti veya iki nokta ile BİTMEYEN bir cümlenin 
        // ardına gelmişse, bu büyük ihtimalle PDF/Web kopyalama hatasıdır (sayfa sonuna denk gelmiştir vb.).
        // Bunu tek bir boşluğa çevirelim.
        normalized = Regex.Replace(normalized, @"([^\.\!\?\:\;\""\'])\n{2,}", "$1 ");

        // 4. Kalan geçerli çift satır sonlarını (paragraf aralarını) belirteç yapalım
        normalized = Regex.Replace(normalized, @"\n{2,}", "[[PARAGRAPH_BREAK]]");

        // 5. Kalan tek satır sonlarını boşluğa çevirelim
        normalized = normalized.Replace("\n", " ");

        // 6. Çoklu boşlukları tek boşluğa indirelim (Kırık paragraf düzeltmesinden kaynaklanmış olabilir)
        normalized = Regex.Replace(normalized, @" {2,}", " ");

        // 7. Paragraf belirteçlerini HTML'e çevirelim
        normalized = normalized.Replace("[[PARAGRAPH_BREAK]]", "<br /><br />");

        return normalized;
    }
}
