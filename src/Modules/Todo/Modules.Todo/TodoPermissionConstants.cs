using FSH.Framework.Shared.Identity;

namespace FSH.Modules.Todo;

/// <summary>
/// Defines all permissions related to Todo management.
/// 
/// Permissions are registered with the framework and used to control access to:
/// - Todo CRUD operations
/// - Task management
/// - Import/Export functionality
/// 
/// All permissions are marked as basic, meaning they are available to all role types.
/// </summary>
public static class TodoPermissionConstants
{
    /// <summary>
    /// Permission constants for Todo-related operations.
    /// </summary>
    public static class Todos
    {
        /// <summary>
        /// Permission to view/retrieve todo items.
        /// </summary>
        public const string View = "Permissions.Todos.View";
        
        /// <summary>
        /// Permission to search and filter todo items.
        /// </summary>
        public const string Search = "Permissions.Todos.Search";
        
        /// <summary>
        /// Permission to create new todo items.
        /// </summary>
        public const string Create = "Permissions.Todos.Create";
        
        /// <summary>
        /// Permission to update existing todo items and their tasks.
        /// </summary>
        public const string Update = "Permissions.Todos.Update";
        
        /// <summary>
        /// Permission to delete todo items.
        /// </summary>
        public const string Delete = "Permissions.Todos.Delete";
        
        /// <summary>
        /// Permission to export todos to file formats.
        /// </summary>
        public const string Export = "Permissions.Todos.Export";
        
        /// <summary>
        /// Permission to import todos from external sources.
        /// </summary>
        public const string Import = "Permissions.Todos.Import";
    }

    /// <summary>
    /// Retrieves all Todo-related permissions for registration with the permission system.
    /// 
    /// Called during module initialization to register permissions with the framework.
    /// </summary>
    /// <returns>A read-only list of all Todo permissions.</returns>
    public static IReadOnlyList<FshPermission> GetPermissions() => new List<FshPermission>
    {
        new("View Todos", ActionConstants.View, ResourceConstants.Todos, IsBasic: true),
        new("Search Todos", ActionConstants.Search, ResourceConstants.Todos, IsBasic: true),
        new("Create Todos", ActionConstants.Create, ResourceConstants.Todos, IsBasic: true),
        new("Update Todos", ActionConstants.Update, ResourceConstants.Todos, IsBasic: true),
        new("Delete Todos", ActionConstants.Delete, ResourceConstants.Todos, IsBasic: true),
        new("Export Todos", ActionConstants.Export, ResourceConstants.Todos, IsBasic: true),
        new("Import Todos", ActionConstants.Import, ResourceConstants.Todos, IsBasic: true),
    };
}
