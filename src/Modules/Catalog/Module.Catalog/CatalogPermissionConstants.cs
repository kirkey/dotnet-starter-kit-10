using FSH.Framework.Shared.Identity;

namespace FSH.Module.Catalog;

/// <summary>
/// Centralized permission constants for the Catalog module.
/// 
/// **Pattern**: Permissions.{Resource}.{Action}
/// Example: Permissions.Catalog.Categories.Create
/// 
/// Permissions are registered in CatalogModule.ConfigureServices()
/// and enforced on endpoints using RequirePermission()
/// </summary>
public static class CatalogPermissionConstants
{
    /// <summary>Permissions for category management.</summary>
    public static class Categories
    {
        public const string View = "Permissions.Catalog.Categories.View";
        public const string Search = "Permissions.Catalog.Categories.Search";
        public const string Create = "Permissions.Catalog.Categories.Create";
        public const string Update = "Permissions.Catalog.Categories.Update";
        public const string Delete = "Permissions.Catalog.Categories.Delete";
    }
    
    /// <summary>Permissions for brand management.</summary>
    public static class Brands
    {
        public const string View = "Permissions.Catalog.Brands.View";
        public const string Search = "Permissions.Catalog.Brands.Search";
        public const string Create = "Permissions.Catalog.Brands.Create";
        public const string Update = "Permissions.Catalog.Brands.Update";
        public const string Delete = "Permissions.Catalog.Brands.Delete";
    }
    
    /// <summary>Permissions for product management.</summary>
    public static class Products
    {
        public const string View = "Permissions.Catalog.Products.View";
        public const string Search = "Permissions.Catalog.Products.Search";
        public const string Create = "Permissions.Catalog.Products.Create";
        public const string Update = "Permissions.Catalog.Products.Update";
        public const string Delete = "Permissions.Catalog.Products.Delete";
    }
    
    /// <summary>
    /// Returns all permissions for the Catalog module.
    /// 
    /// Called in CatalogModule.ConfigureServices() to register all permissions.
    /// </summary>
    public static IReadOnlyList<FshPermission> GetPermissions() => new List<FshPermission>
    {
        new("View Categories", ActionConstants.View, "Catalog.Categories", IsBasic: true),
        new("Search Categories", ActionConstants.Search, "Catalog.Categories", IsBasic: true),
        new("Create Categories", ActionConstants.Create, "Catalog.Categories", IsBasic: true),
        new("Update Categories", ActionConstants.Update, "Catalog.Categories", IsBasic: true),
        new("Delete Categories", ActionConstants.Delete, "Catalog.Categories", IsBasic: true),
        new("View Brands", ActionConstants.View, "Catalog.Brands", IsBasic: true),
        new("Search Brands", ActionConstants.Search, "Catalog.Brands", IsBasic: true),
        new("Create Brands", ActionConstants.Create, "Catalog.Brands", IsBasic: true),
        new("Update Brands", ActionConstants.Update, "Catalog.Brands", IsBasic: true),
        new("Delete Brands", ActionConstants.Delete, "Catalog.Brands", IsBasic: true),
        new("View Products", ActionConstants.View, "Catalog.Products", IsBasic: true),
        new("Search Products", ActionConstants.Search, "Catalog.Products", IsBasic: true),
        new("Create Products", ActionConstants.Create, "Catalog.Products", IsBasic: true),
        new("Update Products", ActionConstants.Update, "Catalog.Products", IsBasic: true),
        new("Delete Products", ActionConstants.Delete, "Catalog.Products", IsBasic: true)
    };
}
