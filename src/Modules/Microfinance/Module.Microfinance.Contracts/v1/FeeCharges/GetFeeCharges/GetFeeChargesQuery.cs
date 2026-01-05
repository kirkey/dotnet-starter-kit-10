using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeCharges.GetFeeCharges;

public sealed record GetFeeChargesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<FeeChargesPagedResponse>;

public sealed record FeeChargesPagedResponse(List<FeeChargeSummaryDto> Items, int TotalCount, int Page, int PageSize);
