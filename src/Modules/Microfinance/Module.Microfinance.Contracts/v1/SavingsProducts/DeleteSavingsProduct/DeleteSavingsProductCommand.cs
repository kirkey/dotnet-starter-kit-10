using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsProducts.DeleteSavingsProduct;

public sealed record DeleteSavingsProductCommand(Guid Id) : ICommand;
