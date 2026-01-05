using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Products;

public record GetProductQuery : IQuery<ProductDto>
{
    public required Guid Id { get; init; }
}
