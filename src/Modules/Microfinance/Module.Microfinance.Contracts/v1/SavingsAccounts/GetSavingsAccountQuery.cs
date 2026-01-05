using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;

/// <summary>
/// Query to get a single savings account by ID.
/// </summary>
public record GetSavingsAccountQuery(Guid Id) : IQuery<SavingsAccountDto>;
