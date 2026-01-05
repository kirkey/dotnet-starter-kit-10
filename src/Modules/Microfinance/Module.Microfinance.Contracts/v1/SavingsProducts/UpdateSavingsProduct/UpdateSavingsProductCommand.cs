using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsProducts.UpdateSavingsProduct;

public sealed record UpdateSavingsProductCommand(Guid Id, string Name) : ICommand<Guid>;
