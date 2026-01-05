using FSH.Module.Accounting.Contracts.v1.BankReconciliations;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations.GetListBankReconciliation;

public record GetBankReconciliationsQuery(
    int Page = 1,
    int PageSize = 10,
    Guid? BankAccountId = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<BankReconciliationsPagedResponse>;

public record BankReconciliationsPagedResponse(
    List<BankReconciliationSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);