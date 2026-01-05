using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentProducts.GetInvestmentProduct;

public sealed record GetInvestmentProductQuery(Guid Id) : IQuery<InvestmentProductDto>;
