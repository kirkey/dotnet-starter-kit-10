using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Brands;

public record CreateBrandCommand : ICommand<Guid>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Website { get; init; }
}
