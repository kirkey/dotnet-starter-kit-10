namespace FSH.Modules.Ai.Services;

public interface IAiTextChunker
{
    IReadOnlyList<string> Chunk(string markdown);
}

/// <summary>
/// Splits Markdown into ~2000-char chunks with 200-char overlap. Packs whole paragraphs;
/// oversized paragraphs hard-split with overlap so no content is lost at boundaries.
/// </summary>
public sealed class AiTextChunker : IAiTextChunker
{
    public const int TargetChars = 2000;
    public const int OverlapChars = 200;

    public IReadOnlyList<string> Chunk(string markdown)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(markdown);

        var paragraphs = markdown
            .Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(p => p.Length > 0)
            .ToList();

        var chunks = new List<string>();
        var current = new System.Text.StringBuilder();
        foreach (var paragraph in paragraphs)
        {
            if (paragraph.Length > TargetChars)
            {
                FlushCurrent(chunks, current);
                chunks.AddRange(HardSplit(paragraph));
            }
            else if (current.Length + paragraph.Length + 2 > TargetChars && current.Length > 0)
            {
                FlushCurrent(chunks, current);
                current.Append(paragraph);
            }
            else
            {
                if (current.Length > 0)
                {
                    current.Append("\n\n");
                }

                current.Append(paragraph);
            }
        }

        FlushCurrent(chunks, current);
        return chunks.Where(c => c.Length > 0).ToList();
    }

    private static void FlushCurrent(List<string> chunks, System.Text.StringBuilder current)
    {
        if (current.Length == 0)
        {
            return;
        }

        chunks.Add(current.ToString());
        var tail = current.ToString();
        current.Clear();
        if (tail.Length > OverlapChars)
        {
            current.Append(tail[^OverlapChars..]);
        }
        else
        {
            current.Append(tail);
        }
    }

    private static IEnumerable<string> HardSplit(string paragraph)
    {
        for (var start = 0; start < paragraph.Length; start += TargetChars - OverlapChars)
        {
            var length = Math.Min(TargetChars, paragraph.Length - start);
            yield return paragraph.Substring(start, length);
            if (start + length >= paragraph.Length)
            {
                yield break;
            }
        }
    }
}
