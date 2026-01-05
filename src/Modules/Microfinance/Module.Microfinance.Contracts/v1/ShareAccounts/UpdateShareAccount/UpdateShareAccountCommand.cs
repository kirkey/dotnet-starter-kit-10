using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareAccounts.UpdateShareAccount;

public sealed record UpdateShareAccountCommand(Guid Id, string Name) : ICommand<Guid>;
