using Mediator;

namespace FSH.Module.Todos.Contracts.v1.Todos;

/// <summary>
/// Query to retrieve a single todo by ID.
/// 
/// **Purpose:**
/// Fetches detailed information about a specific todo item.
/// 
/// **Parameters:**
/// - Id: The GUID of the todo to retrieve
/// 
/// **Response:**
/// Returns a TodoDto containing all details about the todo including task count and completion info.
/// </summary>
public record GetTodoQuery(Guid Id) : IQuery<TodoDto>;
