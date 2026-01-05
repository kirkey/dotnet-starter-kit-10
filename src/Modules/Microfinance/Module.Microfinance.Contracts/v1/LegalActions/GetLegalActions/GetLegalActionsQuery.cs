using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LegalActions.GetLegalActions;

public sealed record GetLegalActionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LegalActionsPagedResponse>;
