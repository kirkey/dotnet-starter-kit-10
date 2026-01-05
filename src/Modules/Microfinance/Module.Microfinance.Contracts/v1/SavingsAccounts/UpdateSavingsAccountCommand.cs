using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;

/// <summary>
/// Command to update an existing savings account.
/// </summary>
public record UpdateSavingsAccountCommand(Guid Id, string Name) : ICommand<Guid>;
