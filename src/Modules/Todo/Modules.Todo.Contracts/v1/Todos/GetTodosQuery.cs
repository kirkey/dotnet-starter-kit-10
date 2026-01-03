using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

/// <summary>
/// Query to retrieve a paginated list of todos with optional filtering.
/// 
/// **Purpose:**
/// Fetches a paginated list of todos with support for searching, filtering by status/priority/completion.
/// 
/// **Parameters:**
/// - Page: Page number for pagination (default: 1)
/// - PageSize: Number of items per page (default: 10)
/// - SearchTerm: Optional search term to filter by name or description
/// - Status: Optional status filter (NotStarted, InProgress, Completed, OnHold)
/// - Priority: Optional priority filter (1-4)
/// - IsCompleted: Optional filter for completed status (true/false)
/// 
/// **Response:**
/// Returns a TodosPagedResponse containing a list of TodoSummaryDto objects and pagination info.
/// </summary>
public record GetTodosQuery(
    /// <summary>
    /// Gets the page number (1-based, default: 1).
    /// </summary>
    int Page = 1,
    
    /// <summary>
    /// Gets the number of items per page (default: 10).
    /// </summary>
    int PageSize = 10,
    
    /// <summary>
    /// Gets the optional search term to filter todos by name or description.
    /// </summary>
    string? SearchTerm = null,
    
    /// <summary>
    /// Gets the optional status filter (NotStarted, InProgress, Completed, OnHold).
    /// </summary>
    string? Status = null,
    
    /// <summary>
    /// Gets the optional priority filter (1=Low, 2=Medium, 3=High, 4=Critical).
    /// </summary>
    int? Priority = null,
    
    /// <summary>
    /// Gets the optional completion status filter.
    /// </summary>
    bool? IsCompleted = null) : IQuery<TodosPagedResponse>;

/// <summary>
/// Response containing paginated todo items.
/// 
/// **Properties:**
/// - Data: List of TodoSummaryDto containing todo summaries
/// - TotalCount: Total number of todos matching the query
/// - Page: Current page number
/// - PageSize: Items per page
/// </summary>
public record TodosPagedResponse(
    /// <summary>
    /// Gets the list of todo summary items for the current page.
    /// </summary>
    List<TodoSummaryDto> Data,
    
    /// <summary>
    /// Gets the total count of todos matching the query filters.
    /// </summary>
    int TotalCount,
    
    /// <summary>
    /// Gets the current page number.
    /// </summary>
    int Page,
    
    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    int PageSize);
