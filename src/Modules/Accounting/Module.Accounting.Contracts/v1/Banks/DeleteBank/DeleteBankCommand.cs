using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Banks.DeleteBank;

public sealed record DeleteBankCommand(Guid Id) : ICommand;