using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareAccounts.CreateShareAccount;

public sealed record CreateShareAccountCommand(string Name) : ICommand<Guid>;
