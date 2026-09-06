namespace FSH.Modules.Ai.Services;

public interface IHtmlToMarkdownConverter
{
    (string Title, string Markdown) Convert(string html, string? fallbackTitle = null);
}
