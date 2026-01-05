using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FixedDeposits.GetFixedDeposits;

public sealed record GetFixedDepositsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<FixedDepositsPagedResponse>;

public sealed record FixedDepositsPagedResponse(List<FixedDepositSummaryDto> Items, int TotalCount, int Page, int PageSize);
