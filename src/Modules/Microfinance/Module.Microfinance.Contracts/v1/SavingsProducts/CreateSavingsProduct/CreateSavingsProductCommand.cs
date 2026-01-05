using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsProducts.CreateSavingsProduct;

public sealed record CreateSavingsProductCommand(string Name) : ICommand<Guid>;
