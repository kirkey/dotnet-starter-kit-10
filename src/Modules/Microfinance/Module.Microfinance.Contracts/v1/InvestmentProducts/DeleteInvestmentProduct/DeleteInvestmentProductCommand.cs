using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentProducts.DeleteInvestmentProduct;

public sealed record DeleteInvestmentProductCommand(Guid Id) : ICommand;
