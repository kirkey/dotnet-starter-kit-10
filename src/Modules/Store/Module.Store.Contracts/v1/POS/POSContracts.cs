using Mediator;

namespace FSH.Module.Store.Contracts.v1.POS;

public record CreatePOSCommand : ICommand<Guid>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string Identifier { get; init; }
    public string? Location { get; init; }
    public required Guid StoreId { get; init; }
}

public record GetPOSByStoreQuery(Guid StoreId) : IQuery<List<POSResponse>>;

public record POSResponse(
    Guid Id,
    string Name,
    string? Description,
    string Identifier,
    string? Location,
    Guid StoreId,
    string StoreName,
    bool IsActive,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);
