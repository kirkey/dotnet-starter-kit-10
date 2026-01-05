namespace FSH.Module.Store;

/// <summary>
/// Centralized permission constants for the Store module.
/// </summary>
public static class StorePermissionConstants
{
    private const string Prefix = "store";
    
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
    public static IReadOnlyList<string> GetPermissions()
    {
        return new List<string>
        {
            Stores.View,
            Stores.Search,
            Stores.Create,
            Stores.Update,
            Stores.Delete,
            POS.View,
            POS.Search,
            POS.Create,
            POS.Update,
            POS.Delete
        };
    }
}
