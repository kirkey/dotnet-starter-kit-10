using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionCases.GetCollectionCases;

public sealed record GetCollectionCasesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollectionCasesPagedResponse>;
