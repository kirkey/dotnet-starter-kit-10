namespace FSH.Framework.Blazor.UI.Components.Navigation.Models;

/// <summary>
/// Represents a single menu item in the navigation menu.
/// </summary>
public class MenuItem
{
    /// <summary>
    /// Display title of the menu item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// MudBlazor icon for the menu item.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Navigation URL/href for the menu item.
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Indicates if this menu item is a parent with child items.
    /// </summary>
    public bool IsParent { get; set; }

    /// <summary>
    /// Child menu items (for hierarchical menus).
    /// </summary>
    public List<MenuItem> MenuItems { get; set; } = new();

    /// <summary>
    /// Development/implementation status of the page.
    /// </summary>
    public PageStatus PageStatus { get; set; } = PageStatus.None;

    /// <summary>
    /// Required permission to view this menu item (optional).
    /// Format: "Permissions.Resource.Action" (e.g., "Permissions.Users.View")
    /// </summary>
    public string? RequiredPermission { get; set; }

    /// <summary>
    /// Additional CSS class(es) to apply to the menu item.
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// Badge text to display on the menu item (e.g., count, "New", etc.).
    /// </summary>
    public string? BadgeText { get; set; }

    /// <summary>
    /// Badge color for the badge text.
    /// </summary>
    public Color BadgeColor { get; set; } = Color.Primary;

    /// <summary>
    /// Whether this menu item should be visible. Defaults to true.
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Target attribute for the link (e.g., "_blank" for new tab).
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Tooltip text to display on hover.
    /// </summary>
    public string? Tooltip { get; set; }
}

