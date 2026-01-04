namespace FSH.Module.Accounting.Contracts.v1.GeneralLedger;

/// <summary>
/// Command to recalculate account balances across the Chart of Accounts or for a single account.
/// </summary>
/// <param name="AccountId">Optional account Id to recalculate; if null recalculates all accounts.</param>
public sealed record RecalculateBalancesCommand(Guid? AccountId = null) : ICommand;