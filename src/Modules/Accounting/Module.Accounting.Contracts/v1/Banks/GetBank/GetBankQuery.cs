using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Banks.GetBank;

public sealed record GetBankQuery(Guid Id) : IQuery<BankDto>;
