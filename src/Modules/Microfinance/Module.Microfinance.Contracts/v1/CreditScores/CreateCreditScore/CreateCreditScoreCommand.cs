using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditScores.CreateCreditScore;

public sealed record CreateCreditScoreCommand(string Name) : ICommand<Guid>;
