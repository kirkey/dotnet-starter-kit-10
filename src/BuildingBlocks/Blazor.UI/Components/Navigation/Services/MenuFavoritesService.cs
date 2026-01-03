using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Text.Json;

namespace FSH.Framework.Blazor.UI.Components.Navigation.Services;

/// <summary>
/// Implementation of menu favorites service using browser local storage.
/// </summary>
public class MenuFavoritesService(
    IJSRuntime jsRuntime,
    IMenuService menuService,
    ILogger<MenuFavoritesService> logger)
    : IMenuFavoritesService
{
    private const string LocalStorageKey = "fsh.menu.favorites";
    private List<string> _favoriteHrefs = new();

    public event EventHandler? FavoritesChanged;

    public async Task<IEnumerable<MenuItem>> GetFavoritesAsync()
    {
        await LoadFavoritesFromStorageAsync();

        var allSections = await menuService.GetMenuSectionsAsync();
        var favoriteItems = new List<MenuItem>();

        foreach (var section in allSections)
        {
            foreach (var item in section.SectionItems)
            {
                if (!string.IsNullOrWhiteSpace(item.Href) && _favoriteHrefs.Contains(item.Href))
                {
                    favoriteItems.Add(item);
                }

                // Check child items
                if (item.IsParent && item.MenuItems.Count > 0)
                {
                    foreach (var childItem in item.MenuItems)
                    {
                        if (!string.IsNullOrWhiteSpace(childItem.Href) && _favoriteHrefs.Contains(childItem.Href))
                        {
                            favoriteItems.Add(childItem);
                        }
                    }
                }
            }
        }

        return favoriteItems;
    }

    public async Task AddToFavoritesAsync(MenuItem menuItem)
    {
        if (string.IsNullOrWhiteSpace(menuItem.Href))
        {
            return;
        }

        await LoadFavoritesFromStorageAsync();

        if (!_favoriteHrefs.Contains(menuItem.Href))
        {
            _favoriteHrefs.Add(menuItem.Href);
            await SaveFavoritesToStorageAsync();
            FavoritesChanged?.Invoke(this, EventArgs.Empty);
            logger.LogInformation("Added {MenuTitle} to favorites", menuItem.Title);
        }
    }

    public async Task RemoveFromFavoritesAsync(string menuItemHref)
    {
        if (string.IsNullOrWhiteSpace(menuItemHref))
        {
            return;
        }

        await LoadFavoritesFromStorageAsync();

        if (_favoriteHrefs.Remove(menuItemHref))
        {
            await SaveFavoritesToStorageAsync();
            FavoritesChanged?.Invoke(this, EventArgs.Empty);
            logger.LogInformation("Removed {Href} from favorites", menuItemHref);
        }
    }

    public async Task<bool> IsFavoriteAsync(string menuItemHref)
    {
        if (string.IsNullOrWhiteSpace(menuItemHref))
        {
            return false;
        }

        await LoadFavoritesFromStorageAsync();
        return _favoriteHrefs.Contains(menuItemHref);
    }

    public async Task<IEnumerable<MenuItemWithFavorite>> GetAllMenuItemsWithFavoriteStatusAsync()
    {
        await LoadFavoritesFromStorageAsync();

        var allSections = await menuService.GetMenuSectionsAsync();
        var result = new List<MenuItemWithFavorite>();

        foreach (var section in allSections)
        {
            foreach (var item in section.SectionItems)
            {
                if (!string.IsNullOrWhiteSpace(item.Href) && !item.IsParent)
                {
                    result.Add(new MenuItemWithFavorite
                    {
                        MenuItem = item,
                        SectionTitle = section.Title,
                        IsFavorite = _favoriteHrefs.Contains(item.Href)
                    });
                }

                // Add child items
                if (item.IsParent && item.MenuItems.Count > 0)
                {
                    foreach (var childItem in item.MenuItems)
                    {
                        if (!string.IsNullOrWhiteSpace(childItem.Href))
                        {
                            result.Add(new MenuItemWithFavorite
                            {
                                MenuItem = childItem,
                                SectionTitle = $"{section.Title} > {item.Title}",
                                IsFavorite = _favoriteHrefs.Contains(childItem.Href)
                            });
                        }
                    }
                }
            }
        }

        return result;
    }

    public async Task ToggleFavoriteAsync(string menuItemHref)
    {
        if (string.IsNullOrWhiteSpace(menuItemHref))
        {
            return;
        }

        await LoadFavoritesFromStorageAsync();

        if (_favoriteHrefs.Contains(menuItemHref))
        {
            await RemoveFromFavoritesAsync(menuItemHref);
        }
        else
        {
            // Find the menu item to add
            var allSections = await menuService.GetMenuSectionsAsync();
            MenuItem? itemToAdd = null;

            foreach (var section in allSections)
            {
                itemToAdd = FindMenuItem(section.SectionItems, menuItemHref);
                if (itemToAdd != null)
                {
                    break;
                }
            }

            if (itemToAdd != null)
            {
                await AddToFavoritesAsync(itemToAdd);
            }
        }
    }

    private static MenuItem? FindMenuItem(List<MenuItem> items, string href)
    {
        foreach (var item in items)
        {
            if (item.Href == href)
            {
                return item;
            }

            if (item.IsParent && item.MenuItems.Count > 0)
            {
                var childResult = FindMenuItem(item.MenuItems, href);
                if (childResult != null)
                {
                    return childResult;
                }
            }
        }

        return null;
    }

    private async Task LoadFavoritesFromStorageAsync()
    {
        try
        {
            var json = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", LocalStorageKey);

            if (!string.IsNullOrWhiteSpace(json))
            {
                _favoriteHrefs = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
        }
        catch (InvalidOperationException)
        {
            // JS interop not available during SSR/static rendering - this is expected
            _favoriteHrefs = new List<string>();
        }
        catch (JSDisconnectedException)
        {
            // Circuit disconnected - ignore
            _favoriteHrefs = new List<string>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load favorites from local storage");
            _favoriteHrefs = new List<string>();
        }
    }

    private async Task SaveFavoritesToStorageAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_favoriteHrefs);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", LocalStorageKey, json);
        }
        catch (InvalidOperationException)
        {
            // JS interop not available during SSR/static rendering - ignore silently
        }
        catch (JSDisconnectedException)
        {
            // Circuit disconnected - ignore silently
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save favorites to local storage");
        }
    }
}

