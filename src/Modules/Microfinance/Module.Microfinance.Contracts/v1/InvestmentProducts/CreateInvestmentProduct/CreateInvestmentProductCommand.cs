using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentProducts.CreateInvestmentProduct;

public sealed record CreateInvestmentProductCommand(string Name) : ICommand<Guid>;
