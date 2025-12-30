namespace FSH.Modules.Todo.Domain;

public static class TodoStatus
{
    public const string Active = "Active";
    public const string Archived = "Archived";
    public const string Completed = "Completed";
}

public static class TodoItemStatus
{
    public const string Pending = "Pending";
    public const string InProgress = "In Progress";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
    public const string OnHold = "On Hold";
}

public enum TodoPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
