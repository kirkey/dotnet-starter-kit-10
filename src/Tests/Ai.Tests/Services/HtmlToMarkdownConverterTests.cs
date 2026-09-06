using FSH.Modules.Ai.Services;
using Shouldly;

namespace Ai.Tests.Services;

public class HtmlToMarkdownConverterTests
{
    private readonly HtmlToMarkdownConverter _sut = new();

    #region Happy Path

    [Fact]
    public void Convert_Should_ExtractTitleAndHeadings()
    {
        const string html = """
            <html><head><title>Test Doc</title></head>
            <body><h1>Hello</h1><p>World of <a href="https://x.test">links</a>.</p></body></html>
            """;

        var (title, markdown) = _sut.Convert(html);

        title.ShouldBe("Test Doc");
        markdown.ShouldContain("# Hello");
        markdown.ShouldContain("World of");
        markdown.ShouldContain("[links](https://x.test)");
    }

    [Fact]
    public void Convert_Should_PreferArticleOverChrome()
    {
        const string html = """
            <html><body><nav>Menu noise</nav>
            <article><p>Real content here.</p></article>
            <footer>Footer noise</footer></body></html>
            """;

        var (_, markdown) = _sut.Convert(html);

        markdown.ShouldContain("Real content here.");
        markdown.ShouldNotContain("Menu noise");
        markdown.ShouldNotContain("Footer noise");
    }

    [Fact]
    public void Convert_Should_StripScriptsAndRenderLists()
    {
        const string html = """
            <html><body><script>alert(1)</script>
            <ul><li>First</li><li>Second</li></ul></body></html>
            """;

        var (_, markdown) = _sut.Convert(html);

        markdown.ShouldNotContain("alert(1)");
        markdown.ShouldContain("- First");
        markdown.ShouldContain("- Second");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Convert_Should_UseFallbackTitle_When_NoTitle()
    {
        var (title, _) = _sut.Convert("<p>Hi</p>", "Fallback");

        title.ShouldBe("Fallback");
    }

    [Fact]
    public void Convert_Should_DecodeEntities()
    {
        var (_, markdown) = _sut.Convert("<p>Fish &amp; Chips</p>");

        markdown.ShouldContain("Fish & Chips");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Convert_Should_RejectEmptyHtml(string html)
    {
        Should.Throw<ArgumentException>(() => _sut.Convert(html));
    }

    #endregion
}
