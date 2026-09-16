using System.Text;
using System.Text.Json;
using System.Web;

namespace DefenceDB.EL.Helpers;

/// <summary>
/// Editor.js JSON çıktısını HTML'e dönüştürür.
/// Tüm temel blok tiplerini destekler.
/// </summary>
public static class EditorJsRenderer
{
    /// <summary>
    /// Editor.js JSON verisini HTML string'e çevirir.
    /// Eğer veri JSON değilse (eski Markdown), null döner.
    /// </summary>
    public static string? RenderToHtml(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        // Eski Markdown verilerini algıla — JSON '{' ile başlar
        var trimmed = json.TrimStart();
        if (!trimmed.StartsWith('{'))
            return null;

        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("blocks", out var blocks))
                return null;

            var sb = new StringBuilder();

            foreach (var block in blocks.EnumerateArray())
            {
                var type = block.GetProperty("type").GetString() ?? "";
                var data = block.GetProperty("data");

                switch (type)
                {
                    case "paragraph":
                        RenderParagraph(sb, data);
                        break;
                    case "header":
                        RenderHeader(sb, data);
                        break;
                    case "list":
                        RenderList(sb, data);
                        break;
                    case "quote":
                        RenderQuote(sb, data);
                        break;
                    case "code":
                        RenderCode(sb, data);
                        break;
                    case "delimiter":
                        sb.AppendLine("<hr />");
                        break;
                    case "table":
                        RenderTable(sb, data);
                        break;
                    case "image":
                        RenderImage(sb, data);
                        break;
                    case "embed":
                        RenderEmbed(sb, data);
                        break;
                    case "warning":
                        RenderWarning(sb, data);
                        break;
                    case "raw":
                        RenderRaw(sb, data);
                        break;
                    case "checklist":
                        RenderChecklist(sb, data);
                        break;
                }
            }

            return sb.ToString();
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>
    /// Verinin Editor.js JSON formatında olup olmadığını kontrol eder.
    /// </summary>
    public static bool IsEditorJsFormat(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return false;

        var trimmed = content.TrimStart();
        if (!trimmed.StartsWith('{'))
            return false;

        try
        {
            using var doc = JsonDocument.Parse(content);
            return doc.RootElement.TryGetProperty("blocks", out _);
        }
        catch
        {
            return false;
        }
    }

    private static void RenderParagraph(StringBuilder sb, JsonElement data)
    {
        var text = GetText(data);
        sb.AppendLine($"<p>{text}</p>");
    }

    private static void RenderHeader(StringBuilder sb, JsonElement data)
    {
        var text = GetText(data);
        var level = data.TryGetProperty("level", out var lvl) ? lvl.GetInt32() : 2;
        level = Math.Clamp(level, 1, 6);
        sb.AppendLine($"<h{level}>{text}</h{level}>");
    }

    private static void RenderList(StringBuilder sb, JsonElement data)
    {
        var style = data.TryGetProperty("style", out var s) ? s.GetString() : "unordered";
        var tag = style == "ordered" ? "ol" : "ul";

        sb.AppendLine($"<{tag}>");
        if (data.TryGetProperty("items", out var items))
        {
            foreach (var item in items.EnumerateArray())
            {
                // items can be strings or objects with "content" property
                string itemText;
                if (item.ValueKind == JsonValueKind.String)
                {
                    itemText = item.GetString() ?? "";
                }
                else if (item.TryGetProperty("content", out var content))
                {
                    itemText = content.GetString() ?? "";
                }
                else
                {
                    itemText = item.GetString() ?? "";
                }
                sb.AppendLine($"  <li>{itemText}</li>");
            }
        }
        sb.AppendLine($"</{tag}>");
    }

    private static void RenderQuote(StringBuilder sb, JsonElement data)
    {
        var text = GetText(data);
        var caption = data.TryGetProperty("caption", out var cap) ? cap.GetString() ?? "" : "";

        sb.AppendLine("<blockquote>");
        sb.AppendLine($"  <p>{text}</p>");
        if (!string.IsNullOrWhiteSpace(caption))
            sb.AppendLine($"  <cite>{Encode(caption)}</cite>");
        sb.AppendLine("</blockquote>");
    }

    private static void RenderCode(StringBuilder sb, JsonElement data)
    {
        var code = data.TryGetProperty("code", out var c) ? c.GetString() ?? "" : "";
        sb.AppendLine($"<pre><code>{Encode(code)}</code></pre>");
    }

    private static void RenderTable(StringBuilder sb, JsonElement data)
    {
        var withHeadings = data.TryGetProperty("withHeadings", out var wh) && wh.GetBoolean();

        if (!data.TryGetProperty("content", out var content))
            return;

        sb.AppendLine("<div class=\"table-responsive\"><table>");

        var isFirstRow = true;
        foreach (var row in content.EnumerateArray())
        {
            if (isFirstRow && withHeadings)
            {
                sb.AppendLine("<thead><tr>");
                foreach (var cell in row.EnumerateArray())
                    sb.AppendLine($"  <th>{cell.GetString() ?? ""}</th>");
                sb.AppendLine("</tr></thead><tbody>");
                isFirstRow = false;
                continue;
            }

            if (isFirstRow)
            {
                sb.AppendLine("<tbody>");
                isFirstRow = false;
            }

            sb.AppendLine("<tr>");
            foreach (var cell in row.EnumerateArray())
                sb.AppendLine($"  <td>{cell.GetString() ?? ""}</td>");
            sb.AppendLine("</tr>");
        }

        sb.AppendLine("</tbody></table></div>");
    }

    private static void RenderImage(StringBuilder sb, JsonElement data)
    {
        var url = data.TryGetProperty("file", out var file)
            ? (file.TryGetProperty("url", out var u) ? u.GetString() ?? "" : "")
            : (data.TryGetProperty("url", out var directUrl) ? directUrl.GetString() ?? "" : "");

        var caption = data.TryGetProperty("caption", out var cap) ? cap.GetString() ?? "" : "";
        var stretched = data.TryGetProperty("stretched", out var st) && st.GetBoolean();
        var withBorder = data.TryGetProperty("withBorder", out var wb) && wb.GetBoolean();
        var withBackground = data.TryGetProperty("withBackground", out var bg) && bg.GetBoolean();

        var classes = new List<string> { "article-editor-image" };
        if (stretched) classes.Add("stretched");
        if (withBorder) classes.Add("with-border");
        if (withBackground) classes.Add("with-background");

        sb.AppendLine($"<figure class=\"{string.Join(' ', classes)}\">");
        sb.AppendLine($"  <img src=\"{Encode(url)}\" alt=\"{Encode(caption)}\" loading=\"lazy\" />");
        if (!string.IsNullOrWhiteSpace(caption))
            sb.AppendLine($"  <figcaption>{caption}</figcaption>");
        sb.AppendLine("</figure>");
    }

    private static void RenderEmbed(StringBuilder sb, JsonElement data)
    {
        var service = data.TryGetProperty("service", out var svc) ? svc.GetString() ?? "" : "";
        var embed = data.TryGetProperty("embed", out var emb) ? emb.GetString() ?? "" : "";
        var caption = data.TryGetProperty("caption", out var cap) ? cap.GetString() ?? "" : "";

        sb.AppendLine("<div class=\"article-embed\">");
        sb.AppendLine($"  <iframe src=\"{Encode(embed)}\" frameborder=\"0\" allowfullscreen loading=\"lazy\"></iframe>");
        if (!string.IsNullOrWhiteSpace(caption))
            sb.AppendLine($"  <p class=\"embed-caption\">{caption}</p>");
        sb.AppendLine("</div>");
    }

    private static void RenderWarning(StringBuilder sb, JsonElement data)
    {
        var title = data.TryGetProperty("title", out var t) ? t.GetString() ?? "" : "";
        var message = data.TryGetProperty("message", out var m) ? m.GetString() ?? "" : "";

        sb.AppendLine("<div class=\"article-warning\">");
        if (!string.IsNullOrWhiteSpace(title))
            sb.AppendLine($"  <strong>{title}</strong>");
        sb.AppendLine($"  <p>{message}</p>");
        sb.AppendLine("</div>");
    }

    private static void RenderRaw(StringBuilder sb, JsonElement data)
    {
        var html = data.TryGetProperty("html", out var h) ? h.GetString() ?? "" : "";
        sb.AppendLine(html);
    }

    private static void RenderChecklist(StringBuilder sb, JsonElement data)
    {
        if (!data.TryGetProperty("items", out var items))
            return;

        sb.AppendLine("<div class=\"article-checklist\">");
        foreach (var item in items.EnumerateArray())
        {
            var text = item.TryGetProperty("text", out var t) ? t.GetString() ?? "" : "";
            var isChecked = item.TryGetProperty("checked", out var c) && c.GetBoolean();
            var checkedAttr = isChecked ? " checked disabled" : " disabled";
            sb.AppendLine($"  <label><input type=\"checkbox\"{checkedAttr} /> {text}</label>");
        }
        sb.AppendLine("</div>");
    }

    private static string GetText(JsonElement data)
    {
        return data.TryGetProperty("text", out var t) ? t.GetString() ?? "" : "";
    }

    private static string Encode(string value)
    {
        return HttpUtility.HtmlEncode(value);
    }
}
