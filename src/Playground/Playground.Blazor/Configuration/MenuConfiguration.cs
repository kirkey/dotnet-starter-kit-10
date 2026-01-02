using FSH.Framework.Blazor.UI.Components.Navigation.Models;

namespace FSH.Playground.Blazor.Configuration;

/// <summary>
/// Defines the navigation menu structure for the Playground application.
/// </summary>
public static class MenuConfiguration
{
    public static IEnumerable<MenuSection> GetMenuSections()
    {
        return new List<MenuSection>
        {
            // Welcome Section
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
                        PageStatus = PageStatus.Completed
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
                        PageStatus = PageStatus.Completed,
                        RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Users)
                    },
                    new MenuItem
                    {
                        Title = "Roles",
                        Icon = Icons.Material.Outlined.AdminPanelSettings,
                        Href = "/roles",
                        PageStatus = PageStatus.Completed,
                        RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Roles)
                    },
                    new MenuItem
                    {
                        Title = "Tenants",
                        Icon = Icons.Material.Outlined.CorporateFare,
                        Href = "/tenants",
                        PageStatus = PageStatus.Completed,
                        RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Tenants)
                    },
                    new MenuItem
                    {
                        Title = "Tenant Settings",
                        Icon = Icons.Material.Outlined.Tune,
                        Href = "/tenants/settings",
                        PageStatus = PageStatus.Completed,
                        RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Tenants)
                    },
                    new MenuItem
                    {
                        Title = "Audit Logs",
                        Icon = Icons.Material.Outlined.History,
                        Href = "/audits",
                        PageStatus = PageStatus.Completed,
                        RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.AuditTrails)
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
                        PageStatus = PageStatus.Completed
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
                        PageStatus = PageStatus.Completed,
                        RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Dashboard)
                    },
                    new MenuItem
                    {
                        Title = "Logs",
                        Icon = Icons.Material.Outlined.Terminal,
                        Href = "/logs",
                        PageStatus = PageStatus.Planned,
                        RequiredPermission = FshPermission.NameFor(ActionConstants.View, ResourceConstants.Dashboard)
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
                        PageStatus = PageStatus.Completed
                    },
                    new MenuItem
                    {
                        Title = "Theme",
                        Icon = Icons.Material.Outlined.Palette,
                        Href = "/settings/theme",
                        PageStatus = PageStatus.Completed
                    },
                    new MenuItem
                    {
                        Title = "Security",
                        Icon = Icons.Material.Outlined.Security,
                        Href = "/settings/security",
                        PageStatus = PageStatus.Completed
                    },
                    new MenuItem
                    {
                        Title = "Sessions",
                        Icon = Icons.Material.Outlined.Devices,
                        Href = "/sessions",
                        PageStatus = PageStatus.Completed
                    },
                    new MenuItem
                    {
                        Title = "About",
                        Icon = Icons.Material.Outlined.Info,
                        Href = "/about",
                        PageStatus = PageStatus.Completed
                    }
                }
            }
        };
    }
}

