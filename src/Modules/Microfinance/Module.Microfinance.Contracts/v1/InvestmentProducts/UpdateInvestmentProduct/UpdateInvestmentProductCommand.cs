using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentProducts.UpdateInvestmentProduct;

public sealed record UpdateInvestmentProductCommand(Guid Id, string Name) : ICommand<Guid>;
