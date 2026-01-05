using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditScores.GetCreditScore;

public sealed record GetCreditScoreQuery(Guid Id) : IQuery<CreditScoreDto>;
