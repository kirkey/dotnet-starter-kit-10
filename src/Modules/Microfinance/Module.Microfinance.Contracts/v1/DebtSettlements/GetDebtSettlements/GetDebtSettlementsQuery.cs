using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.DebtSettlements.GetDebtSettlements;

public sealed record GetDebtSettlementsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<DebtSettlementsPagedResponse>;
