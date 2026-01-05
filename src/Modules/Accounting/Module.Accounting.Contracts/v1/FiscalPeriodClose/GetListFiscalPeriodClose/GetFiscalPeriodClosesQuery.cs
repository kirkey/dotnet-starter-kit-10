using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.GetListFiscalPeriodClose;

public sealed record GetFiscalPeriodCloseQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null, int? FiscalYear = null, Guid? FiscalPeriodId = null, string? Status = null, DateTimeOffset? FromDate = null, DateTimeOffset? ToDate = null) : IQuery<FiscalPeriodClosePagedResponse>;

public sealed record FiscalPeriodClosePagedResponse(List<FiscalPeriodCloseSummaryDto> Items, int TotalCount, int Page, int PageSize);