using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PromiseToPays.GetPromiseToPay;

public sealed record GetPromiseToPayQuery(Guid Id) : IQuery<PromiseToPayDto>;
