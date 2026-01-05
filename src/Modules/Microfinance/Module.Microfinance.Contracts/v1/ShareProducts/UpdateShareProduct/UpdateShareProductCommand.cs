using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareProducts.UpdateShareProduct;

public sealed record UpdateShareProductCommand(Guid Id, string Name) : ICommand<Guid>;
