using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditScores.UpdateCreditScore;

public sealed record UpdateCreditScoreCommand(Guid Id, string Name) : ICommand<Guid>;
