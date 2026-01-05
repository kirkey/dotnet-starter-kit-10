using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareProducts.CreateShareProduct;

public sealed record CreateShareProductCommand(string Name) : ICommand<Guid>;
