using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsProducts.GetSavingsProduct;

public sealed record GetSavingsProductQuery(Guid Id) : IQuery<SavingsProductDto>;
