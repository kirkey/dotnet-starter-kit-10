using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace FSH.Modules.Ai.Services;

/// <summary>
/// Dependency-free HTML → Markdown converter. Prefers &lt;main&gt;/&lt;article&gt; when present,
/// drops script/style/nav/footer, and maps headings, paragraphs, lists, code, quotes, and links
/// to Markdown. Good enough for readable snapshots; not a full-fidelity renderer.
/// </summary>
public sealed partial class HtmlToMarkdownConverter : IHtmlToMarkdownConverter
{
    public (string Title, string Markdown) Convert(string html, string? fallbackTitle = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(html);

        var title = ExtractTitle(html) ?? fallbackTitle ?? "Untitled page";
        var body = ExtractMain(html);
        body = StripTags(body, "script", "style", "noscript", "svg", "nav", "footer", "header", "form", "aside");
        body = ConvertBlocks(body);
        var text = StripRemainingTags(body);
        text = WebUtility.HtmlDecode(text);
        text = NormalizeWhitespace(text);
        return (title.Trim(), text.Trim());
    }

    private static string? ExtractTitle(string html)
    {
        var match = TitleRegex().Match(html);
        return match.Success ? WebUtility.HtmlDecode(match.Groups[1].Value).Trim() : null;
    }

    private static string ExtractMain(string html)
    {
        foreach (var tag in (string[])["article", "main"])
        {
            var match = MainRegex(tag).Match(html);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }

        var body = BodyRegex().Match(html);
        return body.Success ? body.Groups[1].Value : html;
    }

    private static string StripTags(string html, params string[] tags)
    {
        foreach (var tag in tags)
        {
            html = TagStripRegex(tag).Replace(html, string.Empty);
        }

        return html;
    }

    private static string ConvertBlocks(string html)
    {
        var sb = new StringBuilder(html);
        for (var level = 1; level <= 6; level++)
        {
            sb = ReplaceHeading(sb, level);
        }

        sb.Replace("<li>", "\n- ");
        sb.Replace("</li>", string.Empty);
        sb.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n");
        sb.Replace("<p>", "\n\n").Replace("</p>", "\n\n");
        sb.Replace("<pre>", "\n\n```\n").Replace("</pre>", "\n```\n\n");
        sb.Replace("<blockquote>", "\n\n> ").Replace("</blockquote>", "\n\n");
        sb = ReplaceLinks(sb);
        sb.Replace("<code>", "`").Replace("</code>", "`");
        return sb.ToString();
    }

    private static StringBuilder ReplaceHeading(StringBuilder sb, int level)
    {
        var pattern = $@"</?h{level}[^>]*>";
        var replaced = HeadingRegex(pattern).Replace(sb.ToString(), m =>
            m.Value.StartsWith("</", StringComparison.Ordinal) ? "\n\n" : $"\n\n{new string('#', level)} ");
        return new StringBuilder(replaced);
    }

    private static StringBuilder ReplaceLinks(StringBuilder sb)
    {
        var replaced = LinkRegex().Replace(sb.ToString(), m =>
        {
            var text = StripRemainingTags(m.Groups[2].Value).Trim();
            var href = m.Groups[1].Value.Trim();
            return string.IsNullOrEmpty(text) || string.IsNullOrEmpty(href) ? text : $"[{text}]({href})";
        });
        return new StringBuilder(replaced);
    }

    private static string StripRemainingTags(string html) => RemainingTagsRegex().Replace(html, " ");

    private static string NormalizeWhitespace(string text)
    {
        text = BlankLineRegex().Replace(text, "\n\n");
        return text.Split('\n').Select(line => line.Trim()).Aggregate(new StringBuilder(), (sb, line) =>
        {
            if (sb.Length > 0)
            {
                sb.Append('\n');
            }

            return sb.Append(line);
        }).ToString();
    }

    [GeneratedRegex(@"<title[^>]*>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex TitleRegex();

    private static Regex MainRegex(string tag) =>
        new($@"<{tag}[^>]*>(.*?)</{tag}>", RegexOptions.IgnoreCase | RegexOptions.Singleline, TimeSpan.FromSeconds(2));

    private static Regex TagStripRegex(string tag) =>
        new($@"<{tag}[^>]*>.*?</{tag}>", RegexOptions.IgnoreCase | RegexOptions.Singleline, TimeSpan.FromSeconds(2));

    [GeneratedRegex(@"<body[^>]*>(.*?)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex BodyRegex();

    [GeneratedRegex(@"<a[^>]+href\s*=\s*[""']([^""']+)[""'][^>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex LinkRegex();

    [GeneratedRegex(@"<[^>]+>")]
    private static partial Regex RemainingTagsRegex();

    [GeneratedRegex(@"\n{3,}")]
    private static partial Regex BlankLineRegex();

    private static Regex HeadingRegex(string pattern) =>
        new(pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
}
