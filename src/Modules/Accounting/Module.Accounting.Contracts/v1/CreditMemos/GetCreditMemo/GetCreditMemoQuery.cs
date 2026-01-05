using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CreditMemos.GetCreditMemo;

public sealed record GetCreditMemoQuery(Guid Id) : IQuery<CreditMemoDto>;