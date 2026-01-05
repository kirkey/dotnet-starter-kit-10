using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PromiseToPays.DeletePromiseToPay;

public sealed record DeletePromiseToPayCommand(Guid Id) : ICommand;
