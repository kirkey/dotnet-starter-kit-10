using FSH.Framework.Shared.Identity;

namespace FSH.Module.Catalog;

/// <summary>
/// Centralized permission constants for the Catalog module.
/// </summary>
public static class CatalogPermissionConstants
{
    private const string Prefix = "catalog";
    
    public static class ActionConstants
    {
        public const string View = "View";
        public const string Search = "Search";
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Delete = "Delete";
    }
    
    public static class ResourceConstants
    {
        public const string Categories = "Categories";
        public const string Brands = "Brands";
        public const string Products = "Products";
    }
    
    /// <summary>Permissions for category management.</summary>
    public static class Categories
    {
        public const string View = $"{Prefix}:categories:view";
        public const string Search = $"{Prefix}:categories:search";
        public const string Create = $"{Prefix}:categories:create";
        public const string Update = $"{Prefix}:categories:update";
        public const string Delete = $"{Prefix}:categories:delete";
    }
    
    /// <summary>Permissions for brand management.</summary>
    public static class Brands
    {
        public const string View = $"{Prefix}:brands:view";
        public const string Search = $"{Prefix}:brands:search";
        public const string Create = $"{Prefix}:brands:create";
        public const string Update = $"{Prefix}:brands:update";
        public const string Delete = $"{Prefix}:brands:delete";
    }
    
    /// <summary>Permissions for product management.</summary>
    public static class Products
    {
        public const string View = $"{Prefix}:products:view";
        public const string Search = $"{Prefix}:products:search";
        public const string Create = $"{Prefix}:products:create";
        public const string Update = $"{Prefix}:products:update";
        public const string Delete = $"{Prefix}:products:delete";
    }
    
    /// <summary>
    /// Returns all permissions for the Catalog module.
    /// </summary>
    public static IReadOnlyList<FshPermission> GetPermissions() => new List<FshPermission>
    {
        new("View Categories", ActionConstants.View, ResourceConstants.Categories, IsBasic: true),
        new("Search Categories", ActionConstants.Search, ResourceConstants.Categories, IsBasic: true),
        new("Create Categories", ActionConstants.Create, ResourceConstants.Categories),
        new("Update Categories", ActionConstants.Update, ResourceConstants.Categories),
        new("Delete Categories", ActionConstants.Delete, ResourceConstants.Categories),
        new("View Brands", ActionConstants.View, ResourceConstants.Brands, IsBasic: true),
        new("Search Brands", ActionConstants.Search, ResourceConstants.Brands, IsBasic: true),
        new("Create Brands", ActionConstants.Create, ResourceConstants.Brands),
        new("Update Brands", ActionConstants.Update, ResourceConstants.Brands),
        new("Delete Brands", ActionConstants.Delete, ResourceConstants.Brands),
        new("View Products", ActionConstants.View, ResourceConstants.Products, IsBasic: true),
        new("Search Products", ActionConstants.Search, ResourceConstants.Products, IsBasic: true),
        new("Create Products", ActionConstants.Create, ResourceConstants.Products),
        new("Update Products", ActionConstants.Update, ResourceConstants.Products),
        new("Delete Products", ActionConstants.Delete, ResourceConstants.Products)
    };
}
