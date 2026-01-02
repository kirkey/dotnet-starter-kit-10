# Menu Service

The Menu Service provides a flexible, permission-aware navigation menu system for Blazor applications.

## Features

- **Hierarchical Menu Structure**: Support for sections and nested menu items
- **Permission-Based Filtering**: Automatically filters menu items based on user permissions
- **Page Status Indicators**: Show development status (Completed, In Progress, Planned)
- **Badge Support**: Display badges with customizable colors on menu items
- **Icon Support**: Full MudBlazor icon integration
- **Dynamic Updates**: Menu automatically updates when configuration changes

## Usage

### 1. Register the Menu Service

The menu service is automatically registered when you call `AddHeroUI()`:

```csharp
builder.Services.AddHeroUI();
```

### 2. Configure Menu Structure

Create a menu configuration class:

```csharp
using FSH.Framework.Blazor.UI.Components.Navigation.Models;
using FSH.Framework.Shared.Identity;

public static class MenuConfiguration
{
    public static IEnumerable<MenuSection> GetMenuSections()
    {
        return new List<MenuSection>
        {
            new MenuSection
            {
                Title = "My Dashboard",
                Order = 1,
                SectionItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Title = "Personal",
                        Icon = Icons.Material.Filled.Dashboard,
                        Href = "/dashboard/employee",
                        PageStatus = PageStatus.Completed
                    },
                    new MenuItem
                    {
                        Title = "HR",
                        Icon = Icons.Material.Filled.People,
                        Href = "/dashboard/hr",
                        PageStatus = PageStatus.Completed,
                        RequiredPermission = FshPermission.NameFor(FshAction.View, FshResource.Employees)
                    }
                }
            }
        };
    }
}
```

### 3. Initialize Menu in Program.cs

```csharp
var app = builder.Build();

// Initialize menu service
var menuService = app.Services.GetRequiredService<IMenuService>();
menuService.RegisterMenuSections(MenuConfiguration.GetMenuSections());
```

### 4. Use Menu Component

Add the `FshNavMenu` component to your layout:

```razor
@using FSH.Framework.Blazor.UI.Components.Navigation

<nav class="fsh-nav">
    <FshNavMenu />
</nav>
```

## Menu Models

### MenuSection

Represents a section of related menu items.

**Properties:**
- `Title` (string): Section header title
- `SectionItems` (List<MenuItem>): Menu items in this section
- `Order` (int): Display order (lower values appear first)
- `IsVisible` (bool): Whether section is visible
- `Icon` (string?): Optional icon for section header
- `RequiredPermission` (string?): Permission required to view entire section

### MenuItem

Represents a single menu item.

**Properties:**
- `Title` (string): Display title
- `Icon` (string?): MudBlazor icon
- `Href` (string?): Navigation URL
- `IsParent` (bool): Whether item has children
- `MenuItems` (List<MenuItem>): Child menu items
- `PageStatus` (PageStatus): Development status (Completed, InProgress, Planned, None)
- `RequiredPermission` (string?): Permission required to view item
- `BadgeText` (string?): Badge text to display
- `BadgeColor` (Color): Badge color
- `IsVisible` (bool): Whether item is visible
- `Target` (string?): Link target attribute
- `Tooltip` (string?): Tooltip text

### PageStatus Enum

- `Completed`: Feature is fully implemented
- `InProgress`: Feature is being developed
- `Planned`: Feature is planned but not started
- `None`: No status specified

## Permission Integration

The menu service automatically filters items based on user permissions. Permissions are read from the `permission` or `Permission` claims in the authenticated user's identity.

```csharp
new MenuItem
{
    Title = "Users",
    Href = "/users",
    RequiredPermission = "Permissions.Users.View" // Only visible if user has this permission
}
```

## Nested Menus

Create parent-child menu structures:

```csharp
new MenuItem
{
    Title = "Dashboards",
    Icon = Icons.Material.Filled.Dashboard,
    IsParent = true,
    MenuItems = new List<MenuItem>
    {
        new MenuItem { Title = "Personal", Href = "/dashboard/personal" },
        new MenuItem { Title = "Team", Href = "/dashboard/team" }
    }
}
```

## Badges and Status

Add badges and status indicators:

```csharp
new MenuItem
{
    Title = "Notifications",
    Href = "/notifications",
    BadgeText = "5",
    BadgeColor = Color.Error,
    PageStatus = PageStatus.Completed
}
```

## Events

The menu service fires `MenuSectionsChanged` event when menu configuration is updated:

```csharp
menuService.MenuSectionsChanged += async (sender, args) =>
{
    // Handle menu update
};
```

## API Reference

### IMenuService

**Methods:**
- `RegisterMenuSections(IEnumerable<MenuSection> sections)`: Register menu sections
- `Task<IEnumerable<MenuSection>> GetMenuSectionsAsync(IEnumerable<string>? userPermissions)`: Get filtered sections
- `Task<MenuSection?> GetMenuSectionAsync(string title)`: Get specific section by title
- `ClearMenuSections()`: Clear all registered sections

**Events:**
- `MenuSectionsChanged`: Fired when menu configuration changes

