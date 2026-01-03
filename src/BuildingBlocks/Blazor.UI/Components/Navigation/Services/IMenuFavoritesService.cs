namespace FSH.Framework.Blazor.UI.Components.Navigation.Services;

/// <summary>
/// Service for managing user's favorite menu items.
/// </summary>
public interface IMenuFavoritesService
{
    /// <summary>
    /// Gets all favorite menu items for the current user.
    /// </summary>
    Task<IEnumerable<MenuItem>> GetFavoritesAsync();

    /// <summary>
    /// Adds a menu item to favorites.
    /// </summary>
    Task AddToFavoritesAsync(MenuItem menuItem);

    /// <summary>
    /// Removes a menu item from favorites.
    /// </summary>
    Task RemoveFromFavoritesAsync(string menuItemHref);

    /// <summary>
    /// Checks if a menu item is in favorites.
    /// </summary>
    Task<bool> IsFavoriteAsync(string menuItemHref);

    /// <summary>
    /// Gets all menu items with their favorite status.
    /// </summary>
    Task<IEnumerable<MenuItemWithFavorite>> GetAllMenuItemsWithFavoriteStatusAsync();

    /// <summary>
    /// Toggles the favorite status of a menu item.
    /// </summary>
    Task ToggleFavoriteAsync(string menuItemHref);

    /// <summary>
    /// Event triggered when favorites change.
    /// </summary>
    event EventHandler? FavoritesChanged;
}

/// <summary>
/// Menu item with favorite status information.
/// </summary>
public class MenuItemWithFavorite
{
    public required MenuItem MenuItem { get; set; }
    public required string SectionTitle { get; set; }
    public bool IsFavorite { get; set; }
}

