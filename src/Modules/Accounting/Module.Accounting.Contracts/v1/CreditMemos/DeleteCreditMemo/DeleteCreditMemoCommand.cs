using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CreditMemos.DeleteCreditMemo;

public sealed record DeleteCreditMemoCommand(Guid Id) : ICommand;