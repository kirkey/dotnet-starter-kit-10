using Mediator;

namespace FSH.Modules.Todo.Features.v1.Todos.ImportTodos;

/// <summary>
/// Command to import todos
/// </summary>
public sealed record ImportTodosCommand(Stream FileStream, string ContentType) : ICommand<ImportTodosResult>;

/// <summary>
/// Result of import operation
/// </summary>
public sealed record ImportTodosResult(int SuccessCount, int FailureCount, IReadOnlyList<string> Errors);
