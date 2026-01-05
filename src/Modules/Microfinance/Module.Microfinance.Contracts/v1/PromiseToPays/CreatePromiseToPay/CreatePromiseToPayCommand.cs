using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PromiseToPays.CreatePromiseToPay;

public sealed record CreatePromiseToPayCommand(string Name) : ICommand<Guid>;
