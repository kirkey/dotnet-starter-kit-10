using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.CreateInterCompanyTransaction;

public sealed record CreateInterCompanyTransactionCommand(string Name, string? Description = null) : ICommand<Guid>;