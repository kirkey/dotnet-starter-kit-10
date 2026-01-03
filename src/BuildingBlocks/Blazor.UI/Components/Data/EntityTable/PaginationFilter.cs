namespace FSH.Framework.Blazor.UI.Components.Data.EntityTable;

/// <summary>
/// Filter for server-side pagination and search.
/// </summary>
public class PaginationFilter
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public string? Keyword { get; set; }
    public string[] OrderBy { get; set; } = [];
    public Search? AdvancedSearch { get; set; }
}

/// <summary>
/// Advanced search configuration.
/// </summary>
public class Search
{
    public List<string> Fields { get; set; } = [];
}

/// <summary>
/// File upload command for import operations.
/// </summary>
public class FileUploadCommand
{
    public string Name { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long Size { get; set; }
}
