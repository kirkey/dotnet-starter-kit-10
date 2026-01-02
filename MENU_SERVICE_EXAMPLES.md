# Menu Service - Advanced Usage Examples

## Example 1: Custom Module Menu Integration

When creating a new module, register its menu items:

```csharp
// In your module's ServiceCollectionExtensions.cs
public static class InventoryModuleExtensions
{
    public static IServiceCollection AddInventoryModule(this IServiceCollection services)
    {
        // ... other registrations

        // Register module menu items
        services.AddSingleton<IMenuSectionProvider, InventoryMenuProvider>();

        return services;
    }
}

// Create a menu provider for your module
public class InventoryMenuProvider : IMenuSectionProvider
{
    public MenuSection GetMenuSection()
    {
        return new MenuSection
        {
            Title = "Inventory",
            Order = 10,
            SectionItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Title = "Products",
                    Icon = Icons.Material.Filled.Inventory,
                    Href = "/inventory/products",
                    PageStatus = PageStatus.Completed,
                    RequiredPermission = "Permissions.Inventory.View"
                },
                new MenuItem
                {
                    Title = "Warehouses",
                    Icon = Icons.Material.Filled.Warehouse,
                    Href = "/inventory/warehouses",
                    PageStatus = PageStatus.InProgress
                }
            }
        };
    }
}
```

## Example 2: Dynamic Badge Counts

Show notification counts or other dynamic data in badges:

```csharp
public class NotificationMenuProvider
{
    private readonly INotificationService _notificationService;

    public NotificationMenuProvider(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<MenuItem> GetNotificationMenuItem()
    {
        var unreadCount = await _notificationService.GetUnreadCountAsync();

        return new MenuItem
        {
            Title = "Notifications",
            Icon = Icons.Material.Outlined.Notifications,
            Href = "/notifications",
            BadgeText = unreadCount > 0 ? unreadCount.ToString() : null,
            BadgeColor = unreadCount > 0 ? Color.Error : Color.Default,
            PageStatus = PageStatus.Completed
        };
    }
}
```

## Example 3: Conditional Menu Items Based on Features

Show menu items based on feature flags:

```csharp
public class DashboardMenuProvider
{
    private readonly IFeatureManager _featureManager;

    public async Task<MenuSection> GetDashboardSectionAsync()
    {
        var menuItems = new List<MenuItem>
        {
            new MenuItem
            {
                Title = "Overview",
                Icon = Icons.Material.Filled.Dashboard,
                Href = "/dashboard",
                PageStatus = PageStatus.Completed
            }
        };

        // Add analytics menu only if feature is enabled
        if (await _featureManager.IsEnabledAsync("AdvancedAnalytics"))
        {
            menuItems.Add(new MenuItem
            {
                Title = "Analytics",
                Icon = Icons.Material.Filled.Analytics,
                Href = "/dashboard/analytics",
                PageStatus = PageStatus.Completed,
                BadgeText = "New",
                BadgeColor = Color.Primary
            });
        }

        return new MenuSection
        {
            Title = "Dashboards",
            Order = 1,
            SectionItems = menuItems
        };
    }
}
```

## Example 4: Role-Based Menu Sections

Create different menus for different user roles:

```csharp
public class RoleBasedMenuFactory
{
    public IEnumerable<MenuSection> GetMenuSectionsForRole(string role)
    {
        var sections = new List<MenuSection>();

        // Common sections for all users
        sections.Add(GetHomeSect());

        // Admin-specific sections
        if (role == "Admin")
        {
            sections.Add(GetAdminSection());
            sections.Add(GetSystemSection());
        }

        // Manager-specific sections
        if (role == "Manager" || role == "Admin")
        {
            sections.Add(GetReportsSection());
        }

        return sections;
    }

    private MenuSection GetAdminSection()
    {
        return new MenuSection
        {
            Title = "Administration",
            Order = 100,
            SectionItems = new List<MenuItem>
            {
                new MenuItem { Title = "Users", Href = "/admin/users", Icon = Icons.Material.Filled.People },
                new MenuItem { Title = "Settings", Href = "/admin/settings", Icon = Icons.Material.Filled.Settings }
            }
        };
    }
}
```

## Example 5: Multi-Level Nested Menus

Create deep menu hierarchies:

```csharp
new MenuSection
{
    Title = "Reports",
    Order = 50,
    SectionItems = new List<MenuItem>
    {
        new MenuItem
        {
            Title = "Financial Reports",
            Icon = Icons.Material.Filled.Assessment,
            IsParent = true,
            MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Title = "Revenue",
                    Href = "/reports/financial/revenue",
                    Icon = Icons.Material.Filled.TrendingUp
                },
                new MenuItem
                {
                    Title = "Expenses",
                    Href = "/reports/financial/expenses",
                    Icon = Icons.Material.Filled.TrendingDown
                },
                new MenuItem
                {
                    Title = "Profit & Loss",
                    Href = "/reports/financial/pnl",
                    Icon = Icons.Material.Filled.ShowChart,
                    RequiredPermission = "Permissions.FinancialReports.View"
                }
            }
        }
    }
}
```

## Example 6: Menu with Tooltips and External Links

```csharp
new MenuItem
{
    Title = "Documentation",
    Icon = Icons.Material.Filled.Help,
    Href = "https://docs.yourapp.com",
    Target = "_blank",
    Tooltip = "Open documentation in new tab",
    PageStatus = PageStatus.Completed
}
```

## Example 7: Updating Menu Dynamically

Update menu when data changes:

```csharp
public class DynamicMenuService
{
    private readonly IMenuService _menuService;

    public async Task UpdateMenuBasedOnUserContext(string userId)
    {
        // Fetch user-specific menu items from database
        var userMenus = await GetUserCustomMenusAsync(userId);

        // Merge with standard menus
        var allSections = StandardMenus.Concat(userMenus);

        // Update the menu service
        _menuService.RegisterMenuSections(allSections);
    }
}
```

## Example 8: Localized Menu Items

Support multiple languages:

```csharp
public class LocalizedMenuProvider
{
    private readonly IStringLocalizer _localizer;

    public MenuSection GetLocalizedMenu()
    {
        return new MenuSection
        {
            Title = _localizer["Administration"],
            SectionItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Title = _localizer["Users"],
                    Icon = Icons.Material.Filled.People,
                    Href = "/users"
                },
                new MenuItem
                {
                    Title = _localizer["Settings"],
                    Icon = Icons.Material.Filled.Settings,
                    Href = "/settings"
                }
            }
        };
    }
}
```

## Example 9: Menu Item Click Tracking

Track user navigation for analytics:

```csharp
public class AnalyticsMenuWrapper
{
    private readonly IAnalyticsService _analytics;

    public MenuItem WrapWithTracking(MenuItem item)
    {
        var originalHref = item.Href;

        // Wrap the navigation to track clicks
        item.Href = $"/track?destination={originalHref}&item={item.Title}";

        return item;
    }
}
```

## Example 10: Customizing Menu Appearance Per Section

```csharp
new MenuSection
{
    Title = "Premium Features",
    Order = 999,
    Icon = Icons.Material.Filled.Stars,
    SectionItems = new List<MenuItem>
    {
        new MenuItem
        {
            Title = "Advanced Analytics",
            Href = "/premium/analytics",
            CssClass = "premium-menu-item gold-border",
            BadgeText = "PRO",
            BadgeColor = Color.Warning
        }
    }
}
```

## Best Practices

1. **Modular Menu Registration**: Each module should provide its own menu items
2. **Permission Consistency**: Use the same permission constants as in your controllers/handlers
3. **Icon Consistency**: Use a consistent icon library (Material Icons recommended)
4. **Order Values**: Use increments of 10 (10, 20, 30...) to allow easy insertion
5. **Status Updates**: Update PageStatus as features progress through development
6. **Lazy Loading**: For large menus, consider loading sections on-demand
7. **Caching**: Cache menu configurations that don't change frequently
8. **Testing**: Write unit tests for menu filtering logic
9. **Documentation**: Document required permissions for each menu item
10. **Accessibility**: Ensure menu items have meaningful titles and tooltips

## Integration with Other Systems

### With Feature Flags
```csharp
if (await _features.IsEnabledAsync("BetaFeatures"))
{
    sections.Add(GetBetaFeaturesMenu());
}
```

### With User Preferences
```csharp
var favorites = await _userPreferences.GetFavoriteMenuItemsAsync(userId);
sections.Insert(0, new MenuSection
{
    Title = "Favorites",
    Order = 0,
    SectionItems = favorites
});
```

### With A/B Testing
```csharp
var variant = await _abTesting.GetVariantAsync("MenuLayout");
return variant switch
{
    "compact" => GetCompactMenu(),
    "expanded" => GetExpandedMenu(),
    _ => GetDefaultMenu()
};
```

## Example 11: Search and Favorites Integration

The menu service now includes built-in search and favorites functionality:

### Adding Search Bar to Your Layout

```razor
@using FSH.Framework.Blazor.UI.Components.Navigation

<MudAppBar Elevation="0">
    <MudIconButton Icon="@Icons.Material.Filled.Menu" ... />
    <MudSpacer />
    
    <!-- Menu Search with Favorites Button -->
    <div style="max-width: 500px; width: 100%;" class="mx-4">
        <FshMenuSearchBar SearchCssClass="mud-input-outlined-background" />
    </div>
    
    <MudSpacer />
</MudAppBar>
```

### Programmatic Favorites Management

```csharp
@inject IMenuFavoritesService FavoritesService

// Add a menu item to favorites
public async Task AddToFavoritesAsync(MenuItem item)
{
    await FavoritesService.AddToFavoritesAsync(item);
    Snackbar.Add($"{item.Title} added to favorites", Severity.Success);
}

// Remove from favorites
public async Task RemoveFromFavoritesAsync(string href)
{
    await FavoritesService.RemoveFromFavoritesAsync(href);
    Snackbar.Add("Removed from favorites", Severity.Info);
}

// Check if an item is favorited
public async Task<bool> IsItemFavoriteAsync(string href)
{
    return await FavoritesService.IsFavoriteAsync(href);
}

// Get all favorites
public async Task<IEnumerable<MenuItem>> GetUserFavoritesAsync()
{
    return await FavoritesService.GetFavoritesAsync();
}
```

### Custom Favorites Storage Implementation

Replace the default localStorage implementation with server-side storage:

```csharp
public class ServerMenuFavoritesService : IMenuFavoritesService
{
    private readonly HttpClient _http;
    private readonly ILogger<ServerMenuFavoritesService> _logger;
    private List<string> _cachedFavorites = new();

    public event EventHandler? FavoritesChanged;

    public ServerMenuFavoritesService(
        HttpClient http,
        ILogger<ServerMenuFavoritesService> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<IEnumerable<MenuItem>> GetFavoritesAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<List<string>>(
                "api/user/preferences/menu-favorites");
            
            if (response != null)
            {
                _cachedFavorites = response;
            }

            // Convert hrefs to MenuItem objects using MenuService
            // ... implementation
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load favorites from server");
        }

        return Array.Empty<MenuItem>();
    }

    public async Task AddToFavoritesAsync(MenuItem menuItem)
    {
        try
        {
            await _http.PostAsJsonAsync(
                "api/user/preferences/menu-favorites/add",
                new { Href = menuItem.Href });

            _cachedFavorites.Add(menuItem.Href);
            FavoritesChanged?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add favorite to server");
        }
    }

    // Implement other interface methods...
}

// Register in DI
services.AddScoped<IMenuFavoritesService, ServerMenuFavoritesService>();
```

### Favorites Dashboard Widget

Create a dashboard widget showing user's favorite menu items:

```razor
@inject IMenuFavoritesService FavoritesService
@inject NavigationManager Navigation

<MudCard>
    <MudCardHeader>
        <CardHeaderContent>
            <MudText Typo="Typo.h6">
                <MudIcon Icon="@Icons.Material.Filled.Star" Color="Color.Warning" Class="mr-2" />
                Quick Access
            </MudText>
        </CardHeaderContent>
    </MudCardHeader>
    <MudCardContent>
        @if (_favorites == null)
        {
            <MudProgressCircular Indeterminate="true" Size="Size.Small" />
        }
        else if (!_favorites.Any())
        {
            <MudText Typo="Typo.body2" Color="Color.Secondary">
                No favorites yet. Star items from the menu to add them here.
            </MudText>
        }
        else
        {
            <MudList Dense="true">
                @foreach (var item in _favorites.Take(5))
                {
                    <MudListItem Icon="@item.Icon" OnClick="@(() => NavigateTo(item.Href))">
                        @item.Title
                    </MudListItem>
                }
            </MudList>
            
            @if (_favorites.Count() > 5)
            {
                <MudText Typo="Typo.caption" Class="mt-2">
                    And @(_favorites.Count() - 5) more...
                </MudText>
            }
        }
    </MudCardContent>
</MudCard>

@code {
    private IEnumerable<MenuItem>? _favorites;

    protected override async Task OnInitializedAsync()
    {
        _favorites = await FavoritesService.GetFavoritesAsync();
        FavoritesService.FavoritesChanged += OnFavoritesChanged;
    }

    private async void OnFavoritesChanged(object? sender, EventArgs e)
    {
        _favorites = await FavoritesService.GetFavoritesAsync();
        await InvokeAsync(StateHasChanged);
    }

    private void NavigateTo(string? href)
    {
        if (!string.IsNullOrWhiteSpace(href))
        {
            Navigation.NavigateTo(href);
        }
    }

    public void Dispose()
    {
        FavoritesService.FavoritesChanged -= OnFavoritesChanged;
    }
}
```

### Analytics on Favorites

Track which menu items are most favorited:

```csharp
public class FavoritesAnalyticsService
{
    private readonly IMenuFavoritesService _favoritesService;
    private readonly IAnalyticsService _analytics;

    public FavoritesAnalyticsService(
        IMenuFavoritesService favoritesService,
        IAnalyticsService analytics)
    {
        _favoritesService = favoritesService;
        _analytics = analytics;
        
        // Track favorites events
        _favoritesService.FavoritesChanged += OnFavoritesChanged;
    }

    private async void OnFavoritesChanged(object? sender, EventArgs e)
    {
        var favorites = await _favoritesService.GetFavoritesAsync();
        
        await _analytics.TrackEvent("Favorites Updated", new
        {
            FavoriteCount = favorites.Count(),
            TopFavorites = favorites.Take(5).Select(x => x.Title)
        });
    }
}
```

## Integration Tips

1. **Combine with Permissions**: Favorites automatically respect user permissions
2. **Mobile Optimization**: Search bar collapses gracefully on mobile devices
3. **Keyboard Shortcuts**: Consider adding Ctrl+K to focus search
4. **Recent Items**: Combine favorites with recently accessed items
5. **Onboarding**: Suggest favoriting frequently used items during onboarding


