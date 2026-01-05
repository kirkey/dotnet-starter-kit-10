using FSH.Framework.Shared.Identity;

namespace FSH.Module.Store;

/// <summary>
/// Centralized permission constants for the Store module.
/// </summary>
public static class StorePermissionConstants
{
    private const string Prefix = "store";
    
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
        public const string Stores = "Stores";
        public const string POS = "POS";
    }
    
    /// <summary>Permissions for store management.</summary>
    public static class Stores
    {
        public const string View = $"{Prefix}:stores:view";
        public const string Search = $"{Prefix}:stores:search";
        public const string Create = $"{Prefix}:stores:create";
        public const string Update = $"{Prefix}:stores:update";
        public const string Delete = $"{Prefix}:stores:delete";
    }
    
    /// <summary>Permissions for POS management.</summary>
    public static class POS
    {
        public const string View = $"{Prefix}:pos:view";
        public const string Search = $"{Prefix}:pos:search";
        public const string Create = $"{Prefix}:pos:create";
        public const string Update = $"{Prefix}:pos:update";
        public const string Delete = $"{Prefix}:pos:delete";
    }
    
    /// <summary>
    /// Returns all permissions for the Store module.
    /// </summary>
    public static IReadOnlyList<FshPermission> GetPermissions() => new List<FshPermission>
    {
        new("View Stores", ActionConstants.View, ResourceConstants.Stores, IsBasic: true),
        new("Search Stores", ActionConstants.Search, ResourceConstants.Stores, IsBasic: true),
        new("Create Stores", ActionConstants.Create, ResourceConstants.Stores),
        new("Update Stores", ActionConstants.Update, ResourceConstants.Stores),
        new("Delete Stores", ActionConstants.Delete, ResourceConstants.Stores),
        new("View POS", ActionConstants.View, ResourceConstants.POS, IsBasic: true),
        new("Search POS", ActionConstants.Search, ResourceConstants.POS, IsBasic: true),
        new("Create POS", ActionConstants.Create, ResourceConstants.POS),
        new("Update POS", ActionConstants.Update, ResourceConstants.POS),
        new("Delete POS", ActionConstants.Delete, ResourceConstants.POS)
    };
}
