namespace FSH.Framework.Storage;

public enum FileType
{
    Image,
    Document,
    Pdf
}

public sealed class FileValidationRules
{
    public IReadOnlyList<string> AllowedExtensions { get; init; } = Array.Empty<string>();
    public int MaxSizeInMB { get; init; } = 5;
}

public static class FileTypeMetadata
{
    public static FileValidationRules GetRules(FileType type) =>
        type switch
        {
            FileType.Image => new() { AllowedExtensions = [".jpg", ".jpeg", ".png", ".ico"], MaxSizeInMB = 5 },
            FileType.Pdf => new() { AllowedExtensions = [".pdf"], MaxSizeInMB = 10 },
            // Document covers machine-generated text twins (e.g. the Ai module's .md knowledge
            // copies). The enum value predates its rules — without this arm every Document upload
            // throws NotSupportedException.
            FileType.Document => new() { AllowedExtensions = [".md", ".markdown", ".txt"], MaxSizeInMB = 50 },
            _ => throw new NotSupportedException($"Unsupported file type: {type}")
        };
}