namespace FSH.Framework.Blazor.UI.Components.Navigation.Models;

/// <summary>
/// Represents a section in the navigation menu containing related menu items.
/// </summary>
public class MenuSection
{
    /// <summary>
    /// Title of the menu section (displayed as a header).
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// List of menu items in this section.
    /// </summary>
    public List<MenuItem> SectionItems { get; set; } = new();

    /// <summary>
    /// Order/priority for displaying this section (lower values appear first).
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Whether this section should be visible. Defaults to true.
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Icon for the section header (optional).
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Whether the section should be initially collapsed.
    /// </summary>
    public bool IsCollapsible { get; set; }

    /// <summary>
    /// Whether the section is initially collapsed (if IsCollapsible is true).
    /// </summary>
    public bool InitiallyCollapsed { get; set; }

    /// <summary>
    /// Required permission to view this entire section (optional).
    /// If specified, all items in the section require this permission.
    /// </summary>
    public string? RequiredPermission { get; set; }
}

