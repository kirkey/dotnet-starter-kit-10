using Microsoft.Extensions.Logging;

namespace FSH.Framework.Blazor.UI.Components.Navigation.Services;

/// <summary>
/// Default implementation of the menu service.
/// </summary>
public class MenuService(ILogger<MenuService> logger) : IMenuService
{
    private readonly List<MenuSection> _menuSections = new();

    public event EventHandler? MenuSectionsChanged;

    public void RegisterMenuSections(IEnumerable<MenuSection> sections)
    {
        ArgumentNullException.ThrowIfNull(sections);

        _menuSections.Clear();
        _menuSections.AddRange(sections);

        logger.LogInformation("Registered {Count} menu sections", _menuSections.Count);

        MenuSectionsChanged?.Invoke(this, EventArgs.Empty);
    }

    public Task<IEnumerable<MenuSection>> GetMenuSectionsAsync(IEnumerable<string>? userPermissions = null)
    {
        var permissionSet = userPermissions?.ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>();

        var filteredSections = _menuSections
            .Where(section => section.IsVisible)
            .Where(section => HasPermissionForSection(section, permissionSet))
            .Select(section => FilterMenuSection(section, permissionSet))
            .Where(section => section.SectionItems.Count > 0)
            .OrderBy(section => section.Order)
            .ToList();

        return Task.FromResult<IEnumerable<MenuSection>>(filteredSections);
    }

    public Task<MenuSection?> GetMenuSectionAsync(string title)
    {
        var section = _menuSections.FirstOrDefault(s =>
            s.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(section);
    }

    public void ClearMenuSections()
    {
        _menuSections.Clear();
        logger.LogInformation("Cleared all menu sections");
        MenuSectionsChanged?.Invoke(this, EventArgs.Empty);
    }

    private static bool HasPermissionForSection(MenuSection section, HashSet<string> userPermissions)
    {
        // If no permission is required for the section, allow it
        if (string.IsNullOrWhiteSpace(section.RequiredPermission))
        {
            return true;
        }

        // Check if user has the required permission
        return userPermissions.Contains(section.RequiredPermission);
    }

    private MenuSection FilterMenuSection(MenuSection section, HashSet<string> userPermissions)
    {
        // Create a copy of the section to avoid modifying the original
        var filteredSection = new MenuSection
        {
            Title = section.Title,
            Order = section.Order,
            IsVisible = section.IsVisible,
            Icon = section.Icon,
            IsCollapsible = section.IsCollapsible,
            InitiallyCollapsed = section.InitiallyCollapsed,
            RequiredPermission = section.RequiredPermission,
            SectionItems = section.SectionItems
                .Where(item => item.IsVisible)
                .Where(item => HasPermissionForMenuItem(item, userPermissions))
                .Select(item => FilterMenuItem(item, userPermissions))
                .ToList()
        };

        return filteredSection;
    }

    private static bool HasPermissionForMenuItem(MenuItem item, HashSet<string> userPermissions)
    {
        // If no permission is required for the item, allow it
        if (string.IsNullOrWhiteSpace(item.RequiredPermission))
        {
            return true;
        }

        // Check if user has the required permission
        return userPermissions.Contains(item.RequiredPermission);
    }

    private static MenuItem FilterMenuItem(MenuItem item, HashSet<string> userPermissions)
    {
        // Create a copy of the menu item to avoid modifying the original
        var filteredItem = new MenuItem
        {
            Title = item.Title,
            Icon = item.Icon,
            Href = item.Href,
            IsParent = item.IsParent,
            PageStatus = item.PageStatus,
            RequiredPermission = item.RequiredPermission,
            CssClass = item.CssClass,
            BadgeText = item.BadgeText,
            BadgeColor = item.BadgeColor,
            IsVisible = item.IsVisible,
            Target = item.Target,
            Tooltip = item.Tooltip
        };

        // Filter child menu items if this is a parent
        if (item is { IsParent: true, MenuItems.Count: > 0 })
        {
            filteredItem.MenuItems = item.MenuItems
                .Where(childItem => childItem.IsVisible)
                .Where(childItem => HasPermissionForMenuItem(childItem, userPermissions))
                .Select(childItem => FilterMenuItem(childItem, userPermissions))
                .ToList();
        }

        return filteredItem;
    }
}

