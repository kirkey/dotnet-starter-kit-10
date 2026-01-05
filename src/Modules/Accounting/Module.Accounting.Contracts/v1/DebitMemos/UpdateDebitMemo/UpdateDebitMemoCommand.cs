using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DebitMemos.UpdateDebitMemo;

public sealed record UpdateDebitMemoCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;