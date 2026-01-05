using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountReconciliations.GetListAccountReconciliation;

public record GetAccountReconciliationsQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<AccountReconciliationsPagedResponse>;

public record AccountReconciliationsPagedResponse(List<AccountReconciliationSummaryDto> Items, int TotalCount, int Page, int PageSize);