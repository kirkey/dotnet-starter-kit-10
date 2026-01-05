using Mediator;

namespace FSH.Module.Store.Contracts.v1.Stores;

public record CreateStoreCommand : ICommand<Guid>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string Address { get; init; }
    public required string City { get; init; }
    public string? State { get; init; }
    public required string PostalCode { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
}

public record GetStoresQuery : IQuery<List<StoreResponse>>
{
    public string? Search { get; init; }
    public bool IncludeInactive { get; init; }
}

public record StoreResponse(
    Guid Id,
    string Name,
    string? Description,
    string Address,
    string City,
    string? State,
    string PostalCode,
    string? Phone,
    string? Email,
    bool IsActive,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);
