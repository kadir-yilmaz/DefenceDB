namespace DefenceDB.WebUI.Models;

public static class CountryHelper
{
    public static string GetCountryCode(string? countryName)
    {
        if (string.IsNullOrWhiteSpace(countryName)) return "";
        var name = countryName.Trim().ToLowerInvariant()
            .Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u")
            .Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");

        return name switch
        {
            "turkiye" or "turkey" or "tr" => "tr",
            "abd" or "usa" or "united states" or "america" or "amerika" => "us",
            "fransa" or "france" or "fr" => "fr",
            "ingiltere" or "uk" or "united kingdom" or "gb" or "birlesik krallik" => "gb",
            "rusya" or "russia" or "ru" => "ru",
            "almanya" or "germany" or "de" => "de",
            "italya" or "italy" or "it" => "it",
            "isvec" or "sweden" or "se" => "se",
            "cin" or "china" or "cn" => "cn",
            "guney kore" or "south korea" or "kr" or "kore" => "kr",
            "japonya" or "japan" or "jp" => "jp",
            "ukrayna" or "ukraine" or "ua" => "ua",
            "ispanya" or "spain" or "es" => "es",
            "kanada" or "canada" or "ca" => "ca",
            "avustralya" or "australia" or "au" => "au",
            "azerbaycan" or "azerbaijan" or "az" => "az",
            _ => ""
        };
    }

    public static string GetFlagUrl(string? countryName)
    {
        var code = GetCountryCode(countryName);
        if (string.IsNullOrEmpty(code)) return "";
        return $"https://flagcdn.com/w20/{code}.png";
    }

    public class CountryItem
    {
        public string Code { get; set; } = "";
        public string Emoji { get; set; } = "";
        public string Name { get; set; } = "";
    }

    public static List<CountryItem> GetCountries()
    {
        return new List<CountryItem>
        {
            new() { Code = "tr", Emoji = "🇹🇷", Name = "Türkiye" },
            new() { Code = "us", Emoji = "🇺🇸", Name = "ABD" },
            new() { Code = "fr", Emoji = "🇫🇷", Name = "Fransa" },
            new() { Code = "gb", Emoji = "🇬🇧", Name = "İngiltere" },
            new() { Code = "ru", Emoji = "🇷🇺", Name = "Rusya" },
            new() { Code = "de", Emoji = "🇩🇪", Name = "Almanya" },
            new() { Code = "it", Emoji = "🇮🇹", Name = "İtalya" },
            new() { Code = "se", Emoji = "🇸🇪", Name = "İsveç" },
            new() { Code = "cn", Emoji = "🇨🇳", Name = "Çin" },
            new() { Code = "kr", Emoji = "🇰🇷", Name = "Güney Kore" },
            new() { Code = "jp", Emoji = "🇯🇵", Name = "Japonya" },
            new() { Code = "ua", Emoji = "🇺🇦", Name = "Ukrayna" },
            new() { Code = "es", Emoji = "🇪🇸", Name = "İspanya" },
            new() { Code = "ca", Emoji = "🇨🇦", Name = "Kanada" },
            new() { Code = "au", Emoji = "🇦🇺", Name = "Avustralya" },
            new() { Code = "az", Emoji = "🇦🇿", Name = "Azerbaycan" }
        };
    }
}
