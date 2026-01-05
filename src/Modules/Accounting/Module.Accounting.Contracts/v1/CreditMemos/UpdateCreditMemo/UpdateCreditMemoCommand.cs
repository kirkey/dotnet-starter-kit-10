using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CreditMemos.UpdateCreditMemo;

public sealed record UpdateCreditMemoCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;