using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.UpdateInterCompanyTransaction;

public sealed record UpdateInterCompanyTransactionCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;