using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditScores.GetCreditScores;

public sealed record GetCreditScoresQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CreditScoresPagedResponse>;
