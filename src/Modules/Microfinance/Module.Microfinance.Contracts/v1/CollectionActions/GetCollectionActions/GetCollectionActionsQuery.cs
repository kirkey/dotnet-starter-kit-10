using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionActions.GetCollectionActions;

public sealed record GetCollectionActionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollectionActionsPagedResponse>;
