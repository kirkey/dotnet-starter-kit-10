using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareAccounts.DeleteShareAccount;

public sealed record DeleteShareAccountCommand(Guid Id) : ICommand;
