using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;

/// <summary>
/// Command to delete a savings account.
/// </summary>
public record DeleteSavingsAccountCommand(Guid Id) : ICommand<Guid>;
