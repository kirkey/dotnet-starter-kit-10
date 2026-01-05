using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Brands;

/// <summary>
/// Command to create a new brand.
/// </summary>
public record CreateBrandCommand : ICommand<Guid>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? WebsiteUrl { get; init; }
}

/// <summary>
/// Command to update an existing brand.
/// </summary>
public record UpdateBrandCommand : ICommand
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? WebsiteUrl { get; init; }
}

/// <summary>
/// Command to delete a brand.
/// </summary>
public record DeleteBrandCommand(Guid Id) : ICommand;

/// <summary>
/// Query to get a brand by ID.
/// </summary>
public record GetBrandQuery(Guid Id) : IQuery<BrandResponse?>;

/// <summary>
/// Query to get a list of brands with optional filtering.
/// </summary>
public record GetBrandsQuery : IQuery<List<BrandResponse>>
{
    public string? Search { get; init; }
    public bool IncludeInactive { get; init; }
}

/// <summary>
/// Response model for brand details.
/// </summary>
public record BrandResponse(
    Guid Id,
    string Name,
    string? Description,
    string? WebsiteUrl,
    bool IsActive,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);
