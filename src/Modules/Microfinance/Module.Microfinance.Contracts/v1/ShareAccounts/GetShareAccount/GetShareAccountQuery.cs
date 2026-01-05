using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareAccounts.GetShareAccount;

public sealed record GetShareAccountQuery(Guid Id) : IQuery<ShareAccountDto>;
