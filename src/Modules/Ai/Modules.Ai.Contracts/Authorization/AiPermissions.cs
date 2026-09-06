using FSH.Framework.Shared.Constants;

namespace FSH.Modules.Ai.Contracts.Authorization;

public static class AiPermissions
{
    public static class Sources
    {
        public const string Resource = "AiSources";
        public const string View     = $"Permissions.{Resource}.View";
        public const string Create   = $"Permissions.{Resource}.Create";
        public const string Delete   = $"Permissions.{Resource}.Delete";
    }

    public static class Chat
    {
        public const string Resource = "AiChat";
        public const string View     = $"Permissions.{Resource}.View";
        public const string Create   = $"Permissions.{Resource}.Create";
        public const string Delete   = $"Permissions.{Resource}.Delete";
    }

    public static class Agents
    {
        public const string Resource = "AiAgents";
        public const string View     = $"Permissions.{Resource}.View";
        public const string Create   = $"Permissions.{Resource}.Create";
        public const string Update   = $"Permissions.{Resource}.Update";
        public const string Delete   = $"Permissions.{Resource}.Delete";
    }

    public static class Providers
    {
        public const string Resource = "AiProviders";
        public const string View     = $"Permissions.{Resource}.View";
        public const string Manage   = $"Permissions.{Resource}.Manage";
    }

    public static class Runs
    {
        public const string Resource = "AiRuns";
        public const string View     = $"Permissions.{Resource}.View";
        public const string Create   = $"Permissions.{Resource}.Create";
        public const string Cancel   = $"Permissions.{Resource}.Cancel";
    }

    public static IReadOnlyList<FshPermission> All { get; } =
    [
        new("View AI Sources",   ActionConstants.View,   Sources.Resource, IsBasic: true),
        new("Create AI Sources", ActionConstants.Create, Sources.Resource),
        new("Delete AI Sources", ActionConstants.Delete, Sources.Resource),
        new("View AI Chat",      ActionConstants.View,   Chat.Resource, IsBasic: true),
        new("Create AI Chat",    ActionConstants.Create, Chat.Resource),
        new("Delete AI Chat",    ActionConstants.Delete, Chat.Resource),
        new("View AI Agents",    ActionConstants.View,   Agents.Resource),
        new("Create AI Agents",  ActionConstants.Create, Agents.Resource),
        new("Update AI Agents",  ActionConstants.Update, Agents.Resource),
        new("Delete AI Agents",  ActionConstants.Delete, Agents.Resource),
        new("View AI Providers", ActionConstants.View,   Providers.Resource),
        new("Manage AI Providers", "Manage",             Providers.Resource),
        new("View AI Runs",      ActionConstants.View,   Runs.Resource),
        new("Create AI Runs",    ActionConstants.Create, Runs.Resource),
        new("Cancel AI Runs",    "Cancel",               Runs.Resource),
    ];
}
