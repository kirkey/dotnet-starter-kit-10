namespace Accounting.Application.Projects.Costing.Create;

/// <summary>
/// Command to create a new project costing entry.
/// </summary>
/// <param name>The unique identifier of the associated project.</param>
/// <param name>The date when the cost was incurred.</param>
/// <param name>The cost amount (must be positive).</param>
/// <param name>Description of the cost entry.</param>
/// <param name>Reference to the chart of accounts entry.</param>
/// <param name>Optional cost category for classification.</param>
/// <param name>Optional reference to journal entry.</param>
/// <param name>Optional cost center for departmental allocation.</param>
/// <param name>Optional work order number reference.</param>
/// <param name>Whether this cost can be billed to client.</param>
/// <param name>Optional vendor or supplier reference.</param>
/// <param name>Optional invoice or receipt number.</param>
public sealed record CreateProjectCostingCommand(
    DefaultIdType ProjectId,
    DateTime EntryDate,
    decimal Amount,
    string Description,
    DefaultIdType AccountId,
    string? Category,
    DefaultIdType? JournalEntryId,
    string? CostCenter,
    string? WorkOrderNumber,
    bool IsBillable,
    string? Vendor,
    string? InvoiceNumber
) : IRequest<DefaultIdType>;
