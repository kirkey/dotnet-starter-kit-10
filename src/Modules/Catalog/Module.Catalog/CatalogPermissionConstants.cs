namespace FSH.Module.Catalog;

/// <summary>
/// Centralized permission constants for the Catalog module.
/// 
/// **Pattern**: {module}:{feature}:{action}
/// Example: catalog:products:create
/// 
/// Permissions are registered in CatalogModule.ConfigureServices()
/// and enforced on endpoints using RequirePermission()
/// </summary>
public static class CatalogPermissionConstants
{
    /// <summary>Permission prefix for all catalog module permissions.</summary>
    private const string Prefix = "catalog";
    
    /// <summary>Permissions for category management.</summary>
    public static class Categories
    {
        /// <summary>Permission to view categories: catalog:categories:view</summary>
        public const string View = $"{Prefix}:categories:view";
        
        /// <summary>Permission to search categories: catalog:categories:search</summary>
        public const string Search = $"{Prefix}:categories:search";
        
        /// <summary>Permission to create new categories: catalog:categories:create</summary>
        public const string Create = $"{Prefix}:categories:create";
        
        /// <summary>Permission to update existing categories: catalog:categories:update</summary>
        public const string Update = $"{Prefix}:categories:update";
        
        /// <summary>Permission to delete categories: catalog:categories:delete</summary>
        public const string Delete = $"{Prefix}:categories:delete";
    }
    
    /// <summary>Permissions for brand management.</summary>
    public static class Brands
    {
        /// <summary>Permission to view brands: catalog:brands:view</summary>
        public const string View = $"{Prefix}:brands:view";
        
        /// <summary>Permission to search brands: catalog:brands:search</summary>
        public const string Search = $"{Prefix}:brands:search";
        
        /// <summary>Permission to create new brands: catalog:brands:create</summary>
        public const string Create = $"{Prefix}:brands:create";
        
        /// <summary>Permission to update existing brands: catalog:brands:update</summary>
        public const string Update = $"{Prefix}:brands:update";
        
        /// <summary>Permission to delete brands: catalog:brands:delete</summary>
        public const string Delete = $"{Prefix}:brands:delete";
    }
    
    /// <summary>Permissions for product management.</summary>
    public static class Products
    {
        /// <summary>Permission to view products: catalog:products:view</summary>
        public const string View = $"{Prefix}:products:view";
        
        /// <summary>Permission to search products: catalog:products:search</summary>
        public const string Search = $"{Prefix}:products:search";
        
        /// <summary>Permission to create new products: catalog:products:create</summary>
        public const string Create = $"{Prefix}:products:create";
        
        /// <summary>Permission to update existing products: catalog:products:update</summary>
        public const string Update = $"{Prefix}:products:update";
        
        /// <summary>Permission to delete products: catalog:products:delete</summary>
        public const string Delete = $"{Prefix}:products:delete";
    }
    
    /// <summary>
    /// Returns all permissions for the Catalog module.
    /// 
    /// Called in CatalogModule.ConfigureServices() to register all permissions.
    /// </summary>
    public static IReadOnlyList<string> GetPermissions()
    {
        return new List<string>
        {
            Categories.View,
            Categories.Search,
            Categories.Create,
            Categories.Update,
            Categories.Delete,
            Brands.View,
            Brands.Search,
            Brands.Create,
            Brands.Update,
            Brands.Delete,
            Products.View,
            Products.Search,
            Products.Create,
            Products.Update,
            Products.Delete
        };
    }
}
