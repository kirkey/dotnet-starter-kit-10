using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

public record ReorderTasksCommand(Guid TodoId, List<TaskOrderItem> Tasks) : ICommand;

public record TaskOrderItem(Guid TaskId, int SortOrder);
