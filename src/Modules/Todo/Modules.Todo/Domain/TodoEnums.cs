namespace FSH.Modules.Todo.Domain;

public enum TodoStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    OnHold = 3
}

public enum TodoPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
