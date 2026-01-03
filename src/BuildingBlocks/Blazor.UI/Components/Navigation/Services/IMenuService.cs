namespace FSH.Framework.Blazor.UI.Components.Navigation.Services;

/// <summary>
/// Service for managing application navigation menus.
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// Registers menu sections for the application.
    /// </summary>
    /// <param name="sections">Collection of menu sections to register.</param>
    void RegisterMenuSections(IEnumerable<MenuSection> sections);

    /// <summary>
    /// Gets all registered menu sections, optionally filtered by user permissions.
    /// </summary>
    /// <param name="userPermissions">Optional user permissions to filter menu items.</param>
    /// <returns>Collection of menu sections visible to the user.</returns>
    Task<IEnumerable<MenuSection>> GetMenuSectionsAsync(IEnumerable<string>? userPermissions = null);

    /// <summary>
    /// Gets a specific menu section by title.
    /// </summary>
    /// <param name="title">Title of the menu section.</param>
    /// <returns>The menu section if found, otherwise null.</returns>
    Task<MenuSection?> GetMenuSectionAsync(string title);

    /// <summary>
    /// Clears all registered menu sections.
    /// </summary>
    void ClearMenuSections();

    /// <summary>
    /// Event triggered when menu sections are updated.
    /// </summary>
    event EventHandler? MenuSectionsChanged;
}

