using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DebitMemos.CreateDebitMemo;

public sealed record CreateDebitMemoCommand(string Name, string? Description = null) : ICommand<Guid>;