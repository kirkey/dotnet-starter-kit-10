using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CreditMemos.CreateCreditMemo;

public sealed record CreateCreditMemoCommand(string Name, string? Description = null) : ICommand<Guid>;