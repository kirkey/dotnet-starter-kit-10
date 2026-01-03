using Mediator;

namespace FSH.Modules.Todo.Features.v1.Todos.ExportTodos;

/// <summary>
/// Query to export todos
/// </summary>
public sealed record ExportTodosQuery(string Format = "csv") : IQuery<ExportTodosResult>;

/// <summary>
/// Result of export operation
/// </summary>
public sealed record ExportTodosResult(byte[] Data, string ContentType, string FileName);
