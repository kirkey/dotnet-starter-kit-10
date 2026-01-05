using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareProducts.GetShareProduct;

public sealed record GetShareProductQuery(Guid Id) : IQuery<ShareProductDto>;
