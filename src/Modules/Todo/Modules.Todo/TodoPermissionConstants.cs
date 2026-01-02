using FSH.Framework.Shared.Identity;

namespace FSH.Modules.Todo;

public static class TodoPermissionConstants
{
    public static class Todos
    {
        public const string View = "Permissions.Todos.View";
        public const string Search = "Permissions.Todos.Search";
        public const string Create = "Permissions.Todos.Create";
        public const string Update = "Permissions.Todos.Update";
        public const string Delete = "Permissions.Todos.Delete";
        public const string Export = "Permissions.Todos.Export";
        public const string Import = "Permissions.Todos.Import";
    }

    public static IReadOnlyList<FshPermission> GetPermissions() => new List<FshPermission>
    {
        new("View Todos", ActionConstants.View, ResourceConstants.Todos, IsBasic: true),
        new("Search Todos", ActionConstants.Search, ResourceConstants.Todos, IsBasic: true),
        new("Create Todos", ActionConstants.Create, ResourceConstants.Todos),
        new("Update Todos", ActionConstants.Update, ResourceConstants.Todos),
        new("Delete Todos", ActionConstants.Delete, ResourceConstants.Todos),
        new("Export Todos", ActionConstants.Export, ResourceConstants.Todos),
        new("Import Todos", ActionConstants.Import, ResourceConstants.Todos),
    };
}
