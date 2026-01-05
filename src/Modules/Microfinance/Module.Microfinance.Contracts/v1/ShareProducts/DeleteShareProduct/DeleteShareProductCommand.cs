using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareProducts.DeleteShareProduct;

public sealed record DeleteShareProductCommand(Guid Id) : ICommand;
