using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PromiseToPays.UpdatePromiseToPay;

public sealed record UpdatePromiseToPayCommand(Guid Id, string Name) : ICommand<Guid>;
