using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntryLines.CreateJournalEntryLine;

public sealed record CreateJournalEntryLineCommand(
    Guid JournalEntryId,
    int LineNumber,
    Guid AccountId,
    string AccountCode,
    string AccountName,
    decimal Amount,
    string TransactionType,
    string? ReferenceNumber = null,
    string? Description = null,
    string? Notes = null,
    Guid? CostCenterId = null,
    Guid? DepartmentId = null,
    Guid? ProjectId = null) : ICommand<Guid>;
