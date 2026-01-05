using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;

/// <summary>
/// Command to create a new savings account.
/// </summary>
public record CreateSavingsAccountCommand(string Name) : ICommand<Guid>;
