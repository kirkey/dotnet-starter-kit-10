using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditScores.DeleteCreditScore;

public sealed record DeleteCreditScoreCommand(Guid Id) : ICommand;
