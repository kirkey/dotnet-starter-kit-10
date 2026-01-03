using FSH.Framework.Blazor.UI.Components.Navigation.Models;

namespace FSH.Playground.Blazor.Configuration;

/// <summary>
/// Defines the navigation menu structure for the Playground application.
/// 
/// NOTE: Permission-based menu filtering:
/// The RequiredPermission property filters menu items based on JWT claims.
/// By default, permissions are checked server-side (not in JWT claims).
/// To use client-side permission filtering, you need to:
/// 1. Add permissions as claims in the JWT token (see IdentityService.cs)
/// 2. Update the claim type extraction in FshNavMenu.razor if using custom claim types
/// 
/// For now, all menu items are visible and actual authorization is enforced
/// when users try to navigate to restricted pages.
/// </summary>
public static class MenuConfiguration
{
    public static IEnumerable<MenuSection> GetMenuSections()
    {
        return new List<MenuSection>
        {
            // Welcome & Dashboard Section
            new MenuSection
            {
                Title = string.Empty, // No title for welcome section
                Order = 0,
                SectionItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Title = "Welcome",
                        Icon = Icons.Material.Outlined.WavingHand,
                        Href = "/",
                        PageStatus = PageStatus.None
                    },
                    new MenuItem
                    {
                        Title = "Dashboard",
                        Icon = Icons.Material.Outlined.Dashboard,
                        Href = "/dashboard",
                        PageStatus = PageStatus.None
                    },
                    new MenuItem
                    {
                        Title = "Home",
                        Icon = Icons.Material.Outlined.Home,
                        Href = "/home",
                        PageStatus = PageStatus.None
                    }
                }
            },

            // Administration Section
            new MenuSection
            {
                Title = "Administration",
                Order = 1,
                SectionItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Title = "Users",
                        Icon = Icons.Material.Outlined.Person,
                        Href = "/users",
                        PageStatus = PageStatus.None
                        // RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Users)
                        // Note: Uncomment above if permissions are added to JWT claims
                    },
                    new MenuItem
                    {
                        Title = "Roles",
                        Icon = Icons.Material.Outlined.AdminPanelSettings,
                        Href = "/roles",
                        PageStatus = PageStatus.None
                        // RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Roles)
                    },
                    new MenuItem
                    {
                        Title = "Tenants",
                        Icon = Icons.Material.Outlined.CorporateFare,
                        Href = "/tenants",
                        PageStatus = PageStatus.None
                        // RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Tenants)
                    },
                    new MenuItem
                    {
                        Title = "Tenant Settings",
                        Icon = Icons.Material.Outlined.Tune,
                        Href = "/tenants/settings",
                        PageStatus = PageStatus.None
                        // RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Tenants)
                    },
                    new MenuItem
                    {
                        Title = "Audit Logs",
                        Icon = Icons.Material.Outlined.History,
                        Href = "/audits",
                        PageStatus = PageStatus.Completed
                        // RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.AuditTrails)
                    }
                }
            },

            // Communication Section
            new MenuSection
            {
                Title = "Communication",
                Order = 2,
                SectionItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Title = "Chat",
                        Icon = Icons.Material.Outlined.Chat,
                        Href = "/chat",
                        PageStatus = PageStatus.Planned
                    },
                    new MenuItem
                    {
                        Title = "Notifications",
                        Icon = Icons.Material.Outlined.Notifications,
                        Href = "/notifications",
                        PageStatus = PageStatus.Planned
                    }
                }
            },

            // Productivity Section
            new MenuSection
            {
                Title = "Productivity",
                Order = 3,
                SectionItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Title = "Todo Lists",
                        Icon = Icons.Material.Outlined.ChecklistRtl,
                        Href = "/todos",
                        PageStatus = PageStatus.InProgress
                    }
                }
            },

            // System Section
            new MenuSection
            {
                Title = "System",
                Order = 4,
                SectionItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Title = "Health",
                        Icon = Icons.Material.Outlined.MonitorHeart,
                        Href = "/health",
                        PageStatus = PageStatus.None
                        // RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Dashboard)
                    },
                    new MenuItem
                    {
                        Title = "Logs",
                        Icon = Icons.Material.Outlined.Terminal,
                        Href = "/logs",
                        PageStatus = PageStatus.Planned
                        // RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Dashboard)
                    },
                    new MenuItem
                    {
                        Title = "Counter",
                        Icon = Icons.Material.Outlined.Calculate,
                        Href = "/counter",
                        PageStatus = PageStatus.None
                    },
                    new MenuItem
                    {
                        Title = "Weather",
                        Icon = Icons.Material.Outlined.Cloud,
                        Href = "/weather",
                        PageStatus = PageStatus.None
                    }
                }
            },

            // Settings Section
            new MenuSection
            {
                Title = "Settings",
                Order = 5,
                SectionItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Title = "Account",
                        Icon = Icons.Material.Outlined.ManageAccounts,
                        Href = "/settings/profile",
                        PageStatus = PageStatus.None
                    },
                    new MenuItem
                    {
                        Title = "Theme",
                        Icon = Icons.Material.Outlined.Palette,
                        Href = "/settings/theme",
                        PageStatus = PageStatus.None
                    },
                    new MenuItem
                    {
                        Title = "Security",
                        Icon = Icons.Material.Outlined.Security,
                        Href = "/settings/security",
                        PageStatus = PageStatus.Planned
                    },
                    new MenuItem
                    {
                        Title = "Sessions",
                        Icon = Icons.Material.Outlined.Devices,
                        Href = "/sessions",
                        PageStatus = PageStatus.None
                    },
                    new MenuItem
                    {
                        Title = "About",
                        Icon = Icons.Material.Outlined.Info,
                        Href = "/about",
                        PageStatus = PageStatus.Planned
                    }
                }
            }
        };
    }
}

