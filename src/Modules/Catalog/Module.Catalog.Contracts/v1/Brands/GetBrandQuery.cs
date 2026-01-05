using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Brands;

public record GetBrandQuery : IQuery<BrandDto>
{
    public required Guid Id { get; init; }
}
