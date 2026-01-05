using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DebitMemos.GetDebitMemo;

public sealed record GetDebitMemoQuery(Guid Id) : IQuery<FSH.Module.Accounting.Contracts.v1.DebitMemos.DebitMemoDto>;