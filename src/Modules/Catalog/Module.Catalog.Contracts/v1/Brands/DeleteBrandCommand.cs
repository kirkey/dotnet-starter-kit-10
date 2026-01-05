using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Brands;

public record DeleteBrandCommand : ICommand<Unit>
{
    public required Guid Id { get; init; }
}
