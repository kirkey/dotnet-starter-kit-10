using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DebitMemos.DeleteDebitMemo;

public sealed record DeleteDebitMemoCommand(Guid Id) : ICommand;