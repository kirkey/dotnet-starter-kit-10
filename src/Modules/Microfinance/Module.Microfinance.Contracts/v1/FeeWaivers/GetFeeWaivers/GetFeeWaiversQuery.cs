using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeWaivers.GetFeeWaivers;

public sealed record GetFeeWaiversQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<FeeWaiversPagedResponse>;
