using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Products;

public record DeleteProductCommand : ICommand<Unit>
{
    public required Guid Id { get; init; }
}
